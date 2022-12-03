import {Component} from '@angular/core';
import {FormControl, FormGroup, Validators} from "@angular/forms";
import {HttpClient} from "@angular/common/http";
import {Router} from "@angular/router";

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.sass']
})
export class LoginComponent {
  error?: string;

  constructor(private http: HttpClient, private router: Router) {
  }

  loginForm: FormGroup = new FormGroup({
    email: new FormControl('', [
      Validators.required,
      Validators.email
    ]),
    password: new FormControl('', [
      Validators.required
    ]),
    isRemember: new FormControl(true)
  });

  onSubmit() {
    this.http.post('api/users/login', this.loginForm.value).subscribe({
      next: _ => this.router.navigate(['/']),
      error: error => {
        if (error.status === 404 || error.status === 401)
          this.error = "Invalid email or password";
      }
    });
  }
}
