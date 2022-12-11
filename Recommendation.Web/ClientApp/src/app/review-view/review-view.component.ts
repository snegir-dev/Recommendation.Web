import { Component } from '@angular/core';

@Component({
  selector: 'app-review-view',
  templateUrl: './review-view.component.html',
  styleUrls: ['./review-view.component.sass']
})
export class ReviewViewComponent {
  rate: number = 4;
}
