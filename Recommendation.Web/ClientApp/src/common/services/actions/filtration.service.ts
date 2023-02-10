import {Injectable} from "@angular/core";
import {filterBy} from '@progress/kendo-data-query';
import {ReviewCardDto} from "src/common/models/review/reviewCardDto";

@Injectable({
  providedIn: 'root'
})
export class FiltrationService {
  reviews!: ReviewCardDto[];
  private radioGroupClass!: string;
  filtrationText!: string;

  setParams(reviews: ReviewCardDto[], radioGroupClass: string): void {
    this.reviews = reviews;
    this.radioGroupClass = radioGroupClass;
  }

  filter(): ReviewCardDto[] {
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

  getFilteredReviews(filterFieldName: string): ReviewCardDto[] {
    if (!this.filtrationText)
      return this.reviews;

    return filterBy(this.reviews, {
      logic: 'and',
      filters: [{
        field: filterFieldName,
        value: this.filtrationText,
        operator: 'contains',
        ignoreCase: true
      }]
    });
  }
}
