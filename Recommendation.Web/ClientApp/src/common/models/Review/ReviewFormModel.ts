import {FormControl, FormGroup} from "@angular/forms";

export interface ReviewFormModel extends FormGroup {
  controls: {
    image: FormControl,
    nameReview: FormControl,
    nameDescription: FormControl,
    description: FormControl,
    category: FormControl,
    tags: FormControl,
    authorGrade: FormControl
  }
}
