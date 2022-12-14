import {Component, OnInit} from '@angular/core';
import {ReviewService} from "../../common/services/review.service";
import {ReviewDto} from "../../common/models/Review/ReviewDto";
import {ActivatedRoute} from "@angular/router";
import {SortingService} from "../../common/services/actions/sorting.service";

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.sass']
})
export class ProfileComponent implements OnInit {
  constructor(private reviewService: ReviewService,
              private route: ActivatedRoute,
              public sortingService: SortingService) {
  }

  reviews!: ReviewDto[];

  ngOnInit(): void {
    this.fetchReviews();
  }

  fetchReviews() {
    let userId: string = this.getUserIdFromParams();
    this.reviewService.getByUserId(userId).subscribe({
      next: reviews => {
        this.reviews = reviews;
        console.log(reviews)
        this.sortingService.setReviews(reviews);
      }
    });
  }

  getUserIdFromParams(): string {
    let userId: string = '';
    this.route.params.subscribe(params => {
      userId = params['id'];
    });

    return userId;
  }
}
