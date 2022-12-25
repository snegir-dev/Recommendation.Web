import {Component} from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {Router} from "@angular/router";
import {AuthService} from "../../common/services/auths/auth.service";

@Component({
  selector: 'app-login-callback',
  templateUrl: './login-callback.component.html',
  styleUrls: ['./login-callback.component.sass']
})
export class LoginCallbackComponent {
  error!: string;

  constructor(private http: HttpClient,
              private router: Router,
              private authService: AuthService) {

    this.http.get('api/users/external-login-callback').subscribe({
      next: _ => {
        this.authService.isAuthenticate = true;
        this.router.navigate(['/']);
      },
      error: error => this.error = error
    });
  }
}
