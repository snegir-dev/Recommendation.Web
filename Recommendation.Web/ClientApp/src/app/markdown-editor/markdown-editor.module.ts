import {MarkdownEditorComponent} from "./markdown-editor.component";
import {CommonModule} from "@angular/common";
import {CUSTOM_ELEMENTS_SCHEMA, NgModule} from "@angular/core";
import {ReactiveFormsModule} from "@angular/forms";
import {MarkdownModule} from "ngx-markdown";

@NgModule({
  imports: [CommonModule, ReactiveFormsModule, MarkdownModule],
  exports: [MarkdownEditorComponent],
  declarations: [MarkdownEditorComponent],
  schemas: [CUSTOM_ELEMENTS_SCHEMA]
})
export class MarkdownEditorModule {
}
