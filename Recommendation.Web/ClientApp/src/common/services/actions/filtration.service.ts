import {Injectable} from "@angular/core";
import {filterBy} from '@progress/kendo-data-query';
import { ReviewDisplayDto } from "src/common/models/review/review.display.dto";

@Injectable({
  providedIn: 'root'
})
export class FiltrationService {
  reviews!: ReviewDisplayDto[];
  private radioGroupClass!: string;
  private filtrationText!: string;

  setParams(reviews: ReviewDisplayDto[], radioGroupClass: string): void {
    this.reviews = reviews;
    this.radioGroupClass = radioGroupClass;
  }

  filter(): ReviewDisplayDto[] {
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

  getFilteredReviews(filterFieldName: string): ReviewDisplayDto[] {
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
