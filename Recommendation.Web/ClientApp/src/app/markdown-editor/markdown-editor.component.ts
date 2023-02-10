import {Component, HostBinding, Input, OnInit} from '@angular/core';
import '@github/markdown-toolbar-element'
import {FormControl} from "@angular/forms";

@Component({
  selector: 'app-markdown-editor',
  templateUrl: './markdown-editor.component.html',
  styleUrls: ['./markdown-editor.component.sass']
})
export class MarkdownEditorComponent implements OnInit {
  @HostBinding('class.focus') isFocus!: boolean;
  @Input() markdownControl!: FormControl;

  focus() {
    this.isFocus = true;
  }

  blur() {
    this.isFocus = false;
  }

  ngOnInit(): void {
    this.markdownControl = this.markdownControl ?? new FormControl();
  }
}
