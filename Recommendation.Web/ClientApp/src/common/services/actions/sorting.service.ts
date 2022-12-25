import {Injectable} from "@angular/core";
import {sortBy} from 'sort-by-typescript';
import {ReviewCardDto} from "../../models/review/reviewCardDto";

@Injectable({
  providedIn: 'root'
})
export class SortingService {
  reviews!: ReviewCardDto[];

  setReviews(reviews: ReviewCardDto[]) {
    this.reviews = reviews;
  }

  sort(nameField: string, isAscending: boolean) {
    nameField = isAscending ? nameField : `-${nameField}`;
    return this.reviews.sort(sortBy(nameField));
  }
}
