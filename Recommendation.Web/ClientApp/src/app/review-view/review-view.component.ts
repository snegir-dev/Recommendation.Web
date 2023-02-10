import {
  AfterViewChecked,
  AfterViewInit,
  Component,
  ElementRef,
  Input,
  OnDestroy,
  OnInit,
  ViewChild
} from '@angular/core';
import {ActivatedRoute, NavigationEnd, Params, Router} from "@angular/router";
import {RatingService} from "../../common/services/fetches/rating.service";
import {LikeService} from "../../common/services/fetches/like.service";
import {ReviewService} from "../../common/services/fetches/review.service";
import {ReviewModel} from "../../common/models/review/review.model";
import {PdfPrintService} from "../../common/services/prints/pdf.print.service";
import {Subject, takeUntil} from "rxjs";

@Component({
  selector: 'app-review-view',
  templateUrl: './review-view.component.html',
  styleUrls: ['./review-view.component.sass']
})
export class ReviewViewComponent implements OnInit, AfterViewChecked {
  constructor(private reviewService: ReviewService,
              private gradeService: RatingService,
              private likeService: LikeService,
              private route: ActivatedRoute,
              private router: Router,
              public pdfPrintService: PdfPrintService) {
  }

  @ViewChild('pdfSection') pdfSection!: ElementRef;
  waiter!: boolean;
  review!: ReviewModel;
  relatedReviews?: [{ id: string, nameReview: string }];

  ngOnInit(): void {
    this.router.routeReuseStrategy.shouldReuseRoute = () => false;
    this.fetchReview();
    this.fetchRelatedReview();
  }

  ngAfterViewChecked(): void {
    this.pdfPrintService.pdfSection = this.pdfSection;
  }

  fetchReview() {
    let reviewId = this.getReviewId();
    this.reviewService.get(reviewId).subscribe({
      next: value => {
        this.review = value;
        this.waiter = true;
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

  fetchRelatedReview() {
    let reviewId = this.getReviewId();
    this.reviewService.getRelatedReview(reviewId).subscribe({
      next: value => this.relatedReviews = value
    });
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
