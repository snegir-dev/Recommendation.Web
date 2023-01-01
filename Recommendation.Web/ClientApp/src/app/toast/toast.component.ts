import {Component, Input, ViewChild} from '@angular/core';

@Component({
  selector: 'app-toast',
  templateUrl: './toast.component.html',
  styleUrls: ['./toast.component.sass']
})
export class ToastComponent {
  @ViewChild('toast') private toast: any;
  @Input() visible = false;
  @Input() text?: string;
}
