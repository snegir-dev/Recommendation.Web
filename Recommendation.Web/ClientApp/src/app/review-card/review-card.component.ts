import {Component, Input} from '@angular/core';
import {ReviewDto} from "../../common/models/Review/ReviewDto";

@Component({
  selector: 'app-review-card',
  templateUrl: './review-card.component.html',
  styleUrls: ['./review-card.component.sass']
})
export class ReviewCardComponent {
  @Input() reviewPreview!: ReviewDto;
}
