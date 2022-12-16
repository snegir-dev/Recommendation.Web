import {Component, OnInit} from '@angular/core';
import {ReviewFormModel} from "../../common/models/Review/ReviewFormModel";
import {FormControl, FormGroup, Validators} from "@angular/forms";
import {RouterService} from "../../common/services/routers/router.service";
import {ReviewService} from "../../common/services/review.service";
import {ReviewDisplayDto} from "../../common/models/Review/ReviewDisplayDto";
import {ReviewModel} from "../../common/models/Review/ReviewModel";
import {ReviewUpdateDto} from "../../common/models/Review/ReviewUpdateDto";
import {toFormData} from "../../common/functions/toFromData";
import {ActivatedRoute, Route, Router, RouterLinkActive} from "@angular/router";

@Component({
  selector: 'app-update-review',
  templateUrl: './update-review.component.html',
  styleUrls: ['./update-review.component.sass'],
  providers: [RouterService]
})
export class UpdateReviewComponent implements OnInit {
  constructor(private routerService: RouterService,
              private reviewService: ReviewService,
              private router: Router) {
  }

  reviewForm = new FormGroup({
    image: new FormControl<File | null>(null),
    reviewId: new FormControl(''),
    nameReview: new FormControl('', [
      Validators.required,
      Validators.minLength(5)
    ]),
    nameDescription: new FormControl('', [
      Validators.required,
      Validators.minLength(5)
    ]),
    description: new FormControl('', [
      Validators.required,
      Validators.minLength(100)
    ]),
    category: new FormControl('', [
      Validators.required
    ]),
    tags: new FormControl(new Array<string>(), [
      Validators.required
    ]),
    authorGrade: new FormControl(1)
  });

  waiter!: Promise<boolean>;
  reviewId!: string;
  review!: ReviewUpdateDto;

  ngOnInit(): void {
    this.reviewId = this.routerService.getValueFromParams('reviewId');
    this.fetchReview();
  }

  fetchReview(): void {
    this.reviewService.getUpdated(this.reviewId).subscribe({
      next: review => {
        this.review = review;
        this.waiter = Promise.resolve(true);
      }
    });
  }

  onSubmit() {
    this.reviewService.update(toFormData(this.reviewForm.value)).subscribe({
      next: _ => this.router.navigate(['/'])
    })
  }
}
