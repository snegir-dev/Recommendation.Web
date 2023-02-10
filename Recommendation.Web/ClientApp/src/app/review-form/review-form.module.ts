import {CUSTOM_ELEMENTS_SCHEMA, NgModule} from "@angular/core";
import {ReviewFormComponent} from "./review-form.component";
import {NgxDropzoneModule} from "ngx-dropzone";
import {JsonPipe, NgForOf, NgIf} from "@angular/common";
import {FormsModule, ReactiveFormsModule} from "@angular/forms";
import {TagInputModule} from "ngx-chips";
import {MarkdownEditorModule} from "../markdown-editor/markdown-editor.module";
import {NgbRatingModule, NgbTypeaheadModule} from "@ng-bootstrap/ng-bootstrap";
import {TranslateModule} from "@ngx-translate/core";
import {ChoiceCompositionComponent} from "../choice-composition/choice-composition.component";

@NgModule({
  imports: [
    NgxDropzoneModule,
    NgIf,
    ReactiveFormsModule,
    TagInputModule,
    FormsModule,
    MarkdownEditorModule,
    NgbRatingModule,
    NgForOf,
    TranslateModule,
    NgbTypeaheadModule,
    JsonPipe
  ],
  exports: [
    ReviewFormComponent,
    ChoiceCompositionComponent],
  declarations: [
    ReviewFormComponent,
    ChoiceCompositionComponent],
  schemas: [CUSTOM_ELEMENTS_SCHEMA]
})
export class ReviewFormModule {
}
