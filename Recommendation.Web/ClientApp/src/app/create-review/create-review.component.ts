import {Component} from '@angular/core';
import {FormControl, FormGroup, Validators} from "@angular/forms";
import {HttpClient} from "@angular/common/http";
import {ReviewService} from "../../common/services/review.service";
import {CategoryService} from "../../common/services/category.service";
import {ReviewFormModel} from "../../common/models/ReviewFormModel";
import {toFormData} from 'src/common/functions/toFromData';
import {Router} from "@angular/router";

@Component({
  selector: 'app-create-review',
  templateUrl: './create-review.component.html',
  styleUrls: ['./create-review.component.sass']
})
export class CreateReviewComponent {
  constructor(private reviewService: ReviewService,
              private router: Router) {
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
    grade: new FormControl(1)
  });

  onSubmit() {
    this.reviewService.createReview(toFormData(this.reviewForm.value)).subscribe({
      next: _ => this.router.navigate(['/'])
    });
  }
}
