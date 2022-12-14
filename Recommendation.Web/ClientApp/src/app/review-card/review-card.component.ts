import {Component, ElementRef, Input, OnInit, Output, ViewChild} from '@angular/core';
import {ReviewDto} from "../../common/models/Review/ReviewDto";
import {ReviewService} from "../../common/services/review.service";
import {Router} from "@angular/router";
import {RouterService} from "../../common/services/routers/router.service";

@Component({
  selector: 'app-review-card',
  templateUrl: './review-card.component.html',
  styleUrls: ['./review-card.component.sass'],
  providers: [RouterService]
})
export class ReviewCardComponent implements OnInit {
  constructor(private reviewService: ReviewService,
              private router: Router,
              private routerService: RouterService) {
  }

  @Input() reviews!: ReviewDto[];
  @Input() reviewPreview!: ReviewDto;
  @Input() isEdit = false;

  @ViewChild('removeable') private reviewCard!: ElementRef;
  private userId!: string;

  ngOnInit(): void {
    if (this.isEdit)
      this.userId = this.routerService.getValueFromQueryParams('userId');
  }

  deleteReview() {
    this.reviewService.delete(this.reviewPreview.reviewId).subscribe({
      next: _ => {
        const index = this.reviews.indexOf(this.reviewPreview, 0);
        if (index > -1) {
          this.reviews.splice(index, 1);
        }
      }
    });
  }
}
