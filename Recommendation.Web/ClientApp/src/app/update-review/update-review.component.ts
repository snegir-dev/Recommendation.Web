import {Component, OnInit} from '@angular/core';
import {FormControl, FormGroup, Validators} from "@angular/forms";
import {RouterService} from "../../common/services/routers/router.service";
import {Router} from "@angular/router";
import {toFormData} from "../../common/functions/to.from.data";
import {ReviewService} from "../../common/services/fetches/review.service";
import {ReviewUpdateDto} from "../../common/models/review/review.update.dto";
import {ReviewFormModel} from "../../common/models/review/review.form.model";

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

  reviewForm: ReviewFormModel = new FormGroup({
    images: new FormControl<File[]>(new Array<File>(), []),
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

  waiter!: boolean;
  reviewId!: string;
  review!: ReviewUpdateDto;

  ngOnInit() {
    this.reviewId = this.routerService.getValueFromParams('reviewId');
    this.fetchReview();
  }

  fetchReview() {
    this.reviewService.getUpdated(this.reviewId).subscribe({
      next: review => {
        this.review = review;
        this.waiter = true;
      }
    });
  }

  onSubmit() {
    this.waiter = false;
    this.reviewService.update(toFormData(this.reviewForm.value)).subscribe({
      next: _ => this.router.navigate(['/']),
      complete: () => this.waiter = true
    })
  }
}
