import {Component, Input, OnInit, ViewChild} from '@angular/core';
import {FormControl, ValidatorFn, Validators} from "@angular/forms";
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
import {CompositionService} from "../../common/services/fetches/composition.service";

@Component({
  selector: 'app-choice-composition',
  templateUrl: './choice-composition.component.html',
  styleUrls: ['./choice-composition.component.sass']
})

export class ChoiceCompositionComponent implements OnInit {
  constructor(private compositionService: CompositionService) {
  }

  @ViewChild('instance', {static: true}) instance!: NgbTypeahead;
  @Input() compositionFormControl!: FormControl<string>;
  @Input() baseValue?: string;

  compositions: string[] = new Array<string>();
  focus = new Subject<string>();
  click = new Subject<string>();

  ngOnInit(): void {
    this.fetchAllComposition();
  }

  search: OperatorFunction<string, readonly string[]> = (text: Observable<string>) => {
    const debouncedText = text.pipe(debounceTime(200), distinctUntilChanged());
    const clicksWithClosedPopup = this.click.pipe(filter(() => !this.instance.isPopupOpen()));
    const inputFocus = this.focus;

    return merge(debouncedText, inputFocus, clicksWithClosedPopup).pipe(
      map((term) =>
        (term === '' ? this.compositions : this.compositions.filter((v) =>
          v.toLowerCase().indexOf(term.toLowerCase()) > -1)).slice(0, 10),
      ),
    );
  };

  onChange(value: string) {
    this.compositionFormControl.patchValue(value);
  }

  fetchAllComposition() {
    this.compositionService.getAllComposition().subscribe({
      next: compositions => this.compositions = compositions
    })
  }
}
