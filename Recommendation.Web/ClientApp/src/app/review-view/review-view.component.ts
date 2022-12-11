import {Component, OnInit} from '@angular/core';
import {ReviewService} from "../../common/services/review.service";
import {ReviewModel} from "../../common/models/Review/ReviewModel";
import {ActivatedRoute} from "@angular/router";
import {RatingService} from "../../common/services/fetches/rating.service";
import {LikeService} from "../../common/services/fetches/like.service";

@Component({
  selector: 'app-review-view',
  templateUrl: './review-view.component.html',
  styleUrls: ['./review-view.component.sass']
})
export class ReviewViewComponent implements OnInit {
  constructor(private reviewService: ReviewService,
              private gradeService: RatingService,
              private likeService: LikeService,
              private route: ActivatedRoute) {
  }

  rate: number = 4;
  review: ReviewModel = {
    reviewId: '',
    author: '',
    authorGrade: 0,
    description: '',
    nameReview: '',
    tags: new Array<string>(),
    averageCompositionRate: 0,
    category: '',
    nameDescription: '',
    urlImage: '',
    ownSetRating: 0,
    isLike: false
  };

  ngOnInit(): void {
    this.fetchReview();
  }

  fetchReview() {
    let reviewId = this.getReviewId();
    this.reviewService.get(reviewId).subscribe({
      next: value => this.review = value
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
    this.review.isLike = !this.review.isLike;
    this.likeService.setLike({reviewId: this.review.reviewId, isLike: this.review.isLike})
      .subscribe({
        error: _ => this.review.isLike = !this.review.isLike
      });
  }
}
