import {Component, ElementRef, Input, OnInit, Output, ViewChild} from '@angular/core';
import {Router} from "@angular/router";
import {ReviewService} from 'src/common/services/fetches/review.service';
import {RouterService} from "../../common/services/routers/router.service";
import {ReviewCardDto} from "../../common/models/review/reviewCardDto";

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

  @Input() reviews!: ReviewCardDto[];
  @Input() reviewPreview!: ReviewCardDto;
  @Input() isEdit = false;

  @ViewChild('removeable') private reviewCard!: ElementRef;
  waiter: boolean = false;
  userId!: string;

  ngOnInit(): void {
    if (this.isEdit)
      this.userId = this.routerService.getValueFromParams('userId');
  }

  deleteReview() {
    this.waiter = true;
    this.reviewService.delete(this.reviewPreview.reviewId).subscribe({
      next: _ => {
        this.waiter = true;
        const index = this.reviews.indexOf(this.reviewPreview, 0);
        if (index > -1) {
          this.reviews.splice(index, 1);
        }
      },
      error: _ => this.waiter = true
    });
  }
}
