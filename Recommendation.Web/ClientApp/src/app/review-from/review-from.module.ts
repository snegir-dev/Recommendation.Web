import {CUSTOM_ELEMENTS_SCHEMA, NgModule} from "@angular/core";
import {ReviewFromComponent} from "./review-from.component";
import {NgxDropzoneModule} from "ngx-dropzone";
import {NgForOf, NgIf} from "@angular/common";
import {FormsModule, ReactiveFormsModule} from "@angular/forms";
import {TagInputModule} from "ngx-chips";
import {MarkdownEditorModule} from "../markdown-editor/markdown-editor.module";
import {NgbRatingModule} from "@ng-bootstrap/ng-bootstrap";
import {TranslateModule} from "@ngx-translate/core";

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
    TranslateModule
  ],
  exports: [ReviewFromComponent],
  declarations: [ReviewFromComponent],
  schemas: [CUSTOM_ELEMENTS_SCHEMA]
})
export class ReviewFromModule {
}
