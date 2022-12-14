import {Injectable} from "@angular/core";
import {ReviewDto} from "../../models/Review/ReviewDto";
import {filterBy} from '@progress/kendo-data-query';

@Injectable({
  providedIn: 'root'
})
export class FiltrationService {
  reviews!: ReviewDto[];
  private radioGroupClass!: string;
  private filtrationText!: string;

  setParams(reviews: ReviewDto[], radioGroupClass: string): void {
    this.reviews = reviews;
    this.radioGroupClass = radioGroupClass;
  }

  filter(): ReviewDto[] {
    let filterFieldName = this.getFilterFieldName();
    if (!filterFieldName)
      filterFieldName = '';

    return this.getFilteredReviews(filterFieldName);
  }

  onChangeFiltrationText(filtrationText: any) {
    this.filtrationText = filtrationText;
  }

  getFilterFieldName(): string | null | undefined {
    let radioGroup = document.querySelector(`.${this.radioGroupClass}`);
    if (!radioGroup)
      throw new Error('Radio group is null');

    let selectedRadio = radioGroup.querySelector('input[type=radio]:checked')
    return selectedRadio?.getAttribute('filterFieldName');
  }

  getFilteredReviews(filterFieldName: string): ReviewDto[] {
    if (!this.filtrationText)
      return this.reviews;

    return filterBy(this.reviews, {
      logic: 'and',
      filters: [
        {field: filterFieldName, value: this.filtrationText, operator: 'contains', ignoreCase: true}
      ]
    });
  }
}
