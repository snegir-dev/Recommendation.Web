import {Component, Input, OnInit, ViewChild} from '@angular/core';
import {FormControl} from "@angular/forms";
import {
  debounceTime,
  distinctUntilChanged,
  filter,
  map,
  merge,
  Observable,
  OperatorFunction,
  startWith,
  Subject
} from "rxjs";
import {NgbTypeahead} from "@ng-bootstrap/ng-bootstrap";

@Component({
  selector: 'app-choice-composition',
  templateUrl: './choice-composition.component.html',
  styleUrls: ['./choice-composition.component.sass']
})

export class ChoiceCompositionComponent {
  states = [
    'Alabama',
    'Alaska',
    'American Samoa',
    'Arizona',
    'Arkansas',
    'California',
    'Colorado',
    'Connecticut',
    'Delaware',
    'District Of Columbia',
    'Federated States Of Micronesia',
    'Florida',
    'Georgia',
    'Guam',
    'Hawaii',
    'Idaho'
  ];

  @ViewChild('instance', {static: true}) instance!: NgbTypeahead;
  @Input() compositionFormControl!: FormControl<string>;
  @Input() baseValue?: string;

  focus = new Subject<string>();
  click = new Subject<string>();

  search: OperatorFunction<string, readonly string[]> = (text: Observable<string>) => {
    const debouncedText = text.pipe(debounceTime(200), distinctUntilChanged());
    const clicksWithClosedPopup = this.click.pipe(filter(() => !this.instance.isPopupOpen()));
    const inputFocus = this.focus;

    return merge(debouncedText, inputFocus, clicksWithClosedPopup).pipe(
      map((term) =>
        (term === '' ? this.states : this.states.filter((v) =>
          v.toLowerCase().indexOf(term.toLowerCase()) > -1)).slice(0, 10),
      ),
    );
  };

  onChange(value: string) {
    this.compositionFormControl.patchValue(value);
  }
}
