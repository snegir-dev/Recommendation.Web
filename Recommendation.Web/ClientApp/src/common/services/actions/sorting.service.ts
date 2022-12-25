import {Injectable} from "@angular/core";
import {sortBy} from 'sort-by-typescript';
import {ReviewDisplayDto} from "../../models/review/review.display.dto";

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
