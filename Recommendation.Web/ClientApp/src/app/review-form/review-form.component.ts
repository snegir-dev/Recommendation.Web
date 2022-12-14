import {Component, EventEmitter, Input, OnInit, Output} from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {ReviewService} from "../../common/services/review.service";
import {map, Observable} from "rxjs";
import {TagModel} from "ngx-chips/core/tag-model";
import {CategoryService} from "../../common/services/category.service";
import {ReviewFormModel} from "../../common/models/Review/ReviewFormModel";

@Component({
  selector: 'app-review-form',
  templateUrl: './review-form.component.html',
  styleUrls: ['./review-form.component.sass']
})
export class ReviewFormComponent implements OnInit {
  constructor(private reviewService: ReviewService,
              private http: HttpClient, private categoryService: CategoryService) {
  }

  categories!: string[];
  tags: string[] = new Array<string>();
  grade: number = 1;
  file?: File | null = null!;

  @Input() reviewForm!: ReviewFormModel;

  @Output() onSubmitForm = new EventEmitter<boolean>();

  getAllCategories() {
    this.categoryService.getAllCategories().subscribe({
      next: value => this.categories = value.map(function (a: any) {
        return a.category;
      })
    });
  }

  requestAutocompleteItems = (): Observable<any> => {
    return this.http.get('api/tags').pipe(
      map((data: any) => {
        data = data.map(function (a: any) {
          return a.tag;
        });
        return data;
      })
    );
  };

  onAddTag(tag: TagModel) {
    this.tags.push((<any>tag).value);
    this.reviewForm.patchValue({
      tags: this.tags
    });
  }

  onRemoveTag(tag: any) {
    let index = this.tags.indexOf((<any>tag).value);
    if (index !== -1) {
      this.tags.splice(index, 1);
    }
    this.reviewForm.patchValue({
      tags: this.tags
    });
  }

  onGradeChange() {
    this.reviewForm.patchValue({
      grade: this.grade
    });
  }

  onSubmit() {
    this.onSubmitForm.emit();
  }

  onSelectImage(event: any) {
    if (event.addedFiles[0] === undefined)
      return;

    this.file = <File>event.addedFiles[0];
    this.reviewForm.patchValue({
      image: <any>this.file
    });
  }

  onRemoveImage() {
    this.file = null!;
    this.reviewForm.patchValue({
      image: null
    });
  }

  ngOnInit(): void {
    this.getAllCategories();
  }
}
