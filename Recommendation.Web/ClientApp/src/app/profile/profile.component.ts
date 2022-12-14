import {Component, OnInit} from '@angular/core';
import {ReviewService} from "../../common/services/review.service";
import {ReviewDto} from "../../common/models/Review/ReviewDto";
import {SortingService} from "../../common/services/actions/sorting.service";
import {FiltrationService} from "../../common/services/actions/filtration.service";
import {RouterService} from "../../common/services/routers/router.service";

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.sass'],
  providers: [RouterService]
})
export class ProfileComponent implements OnInit {
  constructor(private reviewService: ReviewService,
              private routerService: RouterService,
              public filtrationService: FiltrationService,
              public sortingService: SortingService) {
  }

  reviews!: ReviewDto[];

  ngOnInit(): void {
    this.fetchReviews();
  }

  fetchReviews() {
    let userId: string = this.routerService.getValueFromQueryParams('userId');
    this.reviewService.getByUserId(userId).subscribe({
      next: reviews => {
        this.reviews = reviews;
        this.sortingService.setReviews(reviews);
        this.filtrationService.setParams(this.reviews, 'filtration-container');
      }
    });
  }
}
