import {Component, ElementRef, ViewChild} from '@angular/core';
import {FormControl, FormGroup, Validators} from "@angular/forms";
import {Router} from "@angular/router";
import {RouterService} from "../../common/services/routers/router.service";
import {toFormData} from 'src/common/functions/to.from.data';
import {ReviewFormModel} from 'src/common/models/review/review.form.model';
import {ReviewService} from "../../common/services/fetches/review.service";

@Component({
  selector: 'app-create-review',
  templateUrl: './create-review.component.html',
  styleUrls: ['./create-review.component.sass'],
  providers: [RouterService]
})
export class CreateReviewComponent {
  constructor(private reviewService: ReviewService,
              private routerService: RouterService,
              public router: Router) {
  }

  reviewForm: ReviewFormModel = new FormGroup({
    images: new FormControl<File[]>(new Array<File>(), []),
    nameReview: new FormControl('', [
      Validators.required,
      Validators.minLength(5),
      Validators.maxLength(100)
    ]),
    nameDescription: new FormControl('', [
      Validators.required,
      Validators.minLength(5),
      Validators.maxLength(100)
    ]),
    description: new FormControl('', [
      Validators.required,
      Validators.minLength(100),
      Validators.maxLength(10000)
    ]),
    category: new FormControl('', [
      Validators.required
    ]),
    tags: new FormControl(new Array<string>(), [
      Validators.required
    ]),
    authorGrade: new FormControl(1)
  });

  @ViewChild('generalErrorToast') private generalErrorToast?: any;
  waiter: boolean = true;

  onSubmit() {
    let userId = this.routerService.getValueFromParams<string>('userId') || null;
    this.waiter = false;
    this.reviewService.create(toFormData(this.reviewForm.value), userId).subscribe({
      next: _ => this.router.navigate(['/']),
      error: err => {
        if (err.status == 400)
          this.generalErrorToast.visible = true;
        this.waiter = true;
      },
    });
  }
}
