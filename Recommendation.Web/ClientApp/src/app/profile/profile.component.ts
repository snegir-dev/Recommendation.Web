import {Component, OnInit} from '@angular/core';
import {ReviewService} from "../../common/services/review.service";
import {ReviewDto} from "../../common/models/Review/ReviewDto";
import {ActivatedRoute} from "@angular/router";
import {SortingService} from "../../common/services/actions/sorting.service";
import {FiltrationService} from "../../common/services/actions/filtration.service";

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.sass']
})
export class ProfileComponent implements OnInit {
  constructor(private reviewService: ReviewService,
              private route: ActivatedRoute,
              public filtrationService: FiltrationService,
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
        this.sortingService.setReviews(reviews);
        this.filtrationService.setParams(this.reviews, 'filtration-container');
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
