import {Component} from '@angular/core';
import {FormControl, FormGroup, Validators} from "@angular/forms";
import {HttpClient} from "@angular/common/http";

@Component({
  selector: 'app-create-review',
  templateUrl: './create-review.component.html',
  styleUrls: ['./create-review.component.sass']
})
export class CreateReviewComponent {
  constructor(private http: HttpClient) {
  }

  grade: number = 1;
  file?: File = null!;

  reviewForm = new FormGroup({
    image: new FormControl(null, [
      Validators.required
    ]),
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
    hashtags: new FormControl(''),
    grade: new FormControl(1)
  });

  onGradeChange() {
    this.reviewForm.patchValue({
      grade: this.grade
    });
  }

  onSubmit() {
    console.log(this.reviewForm.value)
  }

  onSelect(event: any) {
    if (event.addedFiles[0] === undefined)
      return;

    this.file = event.addedFiles[0];
    this.reviewForm.patchValue({
      image: <any>this.file
    });
  }

  onRemove() {
    this.file = null!;
    this.reviewForm.patchValue({
      image: null
    });
  }
}
