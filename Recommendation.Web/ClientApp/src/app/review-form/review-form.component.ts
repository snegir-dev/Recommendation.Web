import {Component, EventEmitter, Input, OnInit, Output} from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {map, Observable} from "rxjs";
import {TagModel} from "ngx-chips/core/tag-model";

import {ImageService} from "../../common/services/fetches/image.service";
import {CategoryService} from "../../common/services/fetches/category.service";
import {ReviewFormModel} from "../../common/models/review/review.form.model";
import {ReviewUpdateDto} from "../../common/models/review/review.update.dto";
import {ReviewService} from "../../common/services/fetches/review.service";
import {CompositionService} from "../../common/services/fetches/composition.service";

@Component({
  selector: 'app-review-form',
  templateUrl: './review-form.component.html',
  styleUrls: ['./review-form.component.sass']
})
export class ReviewFormComponent implements OnInit {
  constructor(private reviewService: ReviewService,
              private http: HttpClient,
              private categoryService: CategoryService,
              private imageService: ImageService) {
  }

  categories!: string[];
  tags: string[] = new Array<string>();
  authorGrade: number = 1;
  files: File[] = [];

  @Input() reviewForm!: ReviewFormModel;
  @Input() preloadedReview?: ReviewUpdateDto;
  @Input() nameSubmitButton?: string;
  @Output() onSubmitForm = new EventEmitter<boolean>();

  ngOnInit(): void {
    this.fetchAllCategories();
    this.preloadReview();
  }

  preloadReview(): void {
    if (this.preloadedReview) {
      this.fetchImage();
      this.reviewForm.patchValue({
        reviewId: this.preloadedReview?.reviewId,
        nameReview: this.preloadedReview?.nameReview,
        nameDescription: this.preloadedReview?.nameDescription,
        description: this.preloadedReview?.description,
        category: this.preloadedReview?.category,
        authorGrade: this.preloadedReview?.authorGrade,
        tags: this.preloadedReview?.tags,
      });
      this.authorGrade = this.preloadedReview?.authorGrade!;
      this.tags = this.preloadedReview?.tags!;
    }
  }

  fetchImage() {
    if (this.preloadedReview?.imageMetadatas)
      this.preloadedReview?.imageMetadatas.forEach(i => {
        this.imageService.getImageBlobFromImageMetadata(i).subscribe(value => {
          let file = new File([value.blob], value.fileName);
          this.files.push(file);
          this.reviewForm.patchValue({
            images: file
          });
        });
      })
  }

  fetchAllCategories() {
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
    let index = this.tags.indexOf(tag);
    if (index !== -1) {
      this.tags.splice(index, 1);
    }
    this.reviewForm.patchValue({
      tags: this.tags
    });
  }

  onGradeChange() {
    this.reviewForm.patchValue({
      authorGrade: this.authorGrade
    });
  }

  onSubmit() {
    this.onSubmitForm.emit();
  }

  onSelectImage(event: any) {
    if (event.addedFiles === undefined && event.addedFiles?.length <= 0)
      return;

    event.addedFiles.filter((file: File) => this.files.push(file));
    this.files = Array.from(new Set<string>(this.files.map(f => f.name)))
      .map(n => this.files.filter(f => f.name == n)[0]);
    this.reviewForm.patchValue({
      images: this.files
    });
  }

  onRemoveImage(file: File) {
    this.files = this.files.filter(f => f.name != file.name);
    this.reviewForm.patchValue({
      images: this.files
    });
  }

  onUploadFileInvalid(event: any) {
    console.log(event);
  }
}
