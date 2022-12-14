import {Injectable} from "@angular/core";
import {ReviewDto} from "../../models/Review/ReviewDto";
import {sortBy} from 'sort-by-typescript';

@Injectable({
  providedIn: 'root'
})
export class SortingService {
  reviews!: ReviewDto[];

  setReviews(reviews: ReviewDto[]) {
    this.reviews = reviews;
  }

  sort(nameField: string, isAscending: boolean) {
    nameField = isAscending ? nameField : `-${nameField}`;
    return this.reviews.sort(sortBy(nameField));
  }
}
