import {Component, OnInit} from '@angular/core';
import {ActivatedRoute} from "@angular/router";
import {RatingService} from "../../common/services/fetches/rating.service";
import {LikeService} from "../../common/services/fetches/like.service";
import {ReviewService} from "../../common/services/fetches/review.service";
import {ReviewModel} from "../../common/models/review/review.model";
import {PdfPrintService} from "../../common/services/prints/pdf.print.service";

@Component({
  selector: 'app-review-view',
  templateUrl: './review-view.component.html',
  styleUrls: ['./review-view.component.sass']
})
export class ReviewViewComponent implements OnInit {
  constructor(private reviewService: ReviewService,
              private gradeService: RatingService,
              private likeService: LikeService,
              private route: ActivatedRoute,
              public pdfPrintService: PdfPrintService) {
  }

  waiter!: Promise<boolean>;
  review!: ReviewModel;

  ngOnInit(): void {
    this.fetchReview();
  }

  fetchReview() {
    let reviewId = this.getReviewId();
    this.reviewService.get(reviewId).subscribe({
      next: value => {
        this.review = value;
        this.waiter = Promise.resolve(true);
      }
    });
  }

  getReviewId(): string {
    let id: string = '';
    this.route.params.subscribe(params => {
      id = params['id'];
    });

    return id;
  }

  changeRate(gradeValue: any) {
    if (gradeValue)
      this.gradeService.setRating({reviewId: this.review.reviewId, gradeValue: gradeValue})
        .subscribe({
          next: value => console.log(value),
          error: err => console.log(err)
        });
  }

  changeLike() {
    this.review.countLike = this.review.isLike ? this.review.countLike - 1 : this.review.countLike + 1
    this.review.isLike = !this.review.isLike;
    this.likeService.setLike({reviewId: this.review.reviewId, isLike: this.review.isLike})
      .subscribe({
        error: _ => {
          this.review.countLike = this.review.isLike ? this.review.countLike - 1 : this.review.countLike + 1
          this.review.isLike = !this.review.isLike;
        }
      });
  }
}
