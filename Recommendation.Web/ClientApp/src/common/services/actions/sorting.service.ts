import {Injectable} from "@angular/core";
import {ReviewDisplayDto} from "../../models/Review/ReviewDisplayDto";
import {sortBy} from 'sort-by-typescript';

@Injectable({
  providedIn: 'root'
})
export class SortingService {
  reviews!: ReviewDisplayDto[];

  setReviews(reviews: ReviewDisplayDto[]) {
    this.reviews = reviews;
  }

  sort(nameField: string, isAscending: boolean) {
    nameField = isAscending ? nameField : `-${nameField}`;
    return this.reviews.sort(sortBy(nameField));
  }
}
