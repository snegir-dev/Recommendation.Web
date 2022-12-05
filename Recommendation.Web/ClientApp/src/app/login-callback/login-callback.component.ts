import {Component, OnInit} from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {Router} from "@angular/router";

@Component({
  selector: 'app-login-callback',
  templateUrl: './login-callback.component.html',
  styleUrls: ['./login-callback.component.sass']
})
export class LoginCallbackComponent {
  error!: string;

  constructor(private http: HttpClient, private router: Router) {
    this.http.get('api/users/external-login-callback').subscribe({
      next: _ => this.router.navigate(['/']),
      error: error => this.error = error
    });
  }
}
