import {Component, OnInit, Output} from '@angular/core';
import {FormControl, FormGroup, Validators} from "@angular/forms";
import {ReviewService} from "../../common/services/review.service";
import {Router} from "@angular/router";
import {toFormData} from "../../common/functions/toFromData";
import {ReviewFormModel} from "../../common/models/Review/ReviewFormModel";

@Component({
  selector: 'app-create-review',
  templateUrl: './create-review.component.html',
  styleUrls: ['./create-review.component.sass']
})
export class CreateReviewComponent {
  constructor(private reviewService: ReviewService,
              public router: Router) {
  }

  reviewForm: ReviewFormModel = new FormGroup({
    image: new FormControl(null),
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

  onSubmit() {
    this.reviewService.create(toFormData(this.reviewForm.value)).subscribe({
      next: _ => this.router.navigate(['/'])
    });
  }
}
