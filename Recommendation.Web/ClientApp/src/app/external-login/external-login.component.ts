import {Component} from '@angular/core';

@Component({
  selector: 'app-external-login',
  templateUrl: './external-login.component.html',
  styleUrls: ['./external-login.component.sass']
})
export class ExternalLoginComponent {
  externalLogin(provider: string) {
    window.location.href = `api/users/external-login?provider=${provider}`;
  }
}
