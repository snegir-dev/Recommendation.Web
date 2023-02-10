import {Component, ViewChild} from '@angular/core';
import {FormControl, FormGroup, Validators} from "@angular/forms";
import {HttpClient} from "@angular/common/http";
import {Router} from "@angular/router";
import {AuthService} from "../../common/services/auths/auth.service";

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.sass']
})
export class LoginComponent {
  error?: string;

  constructor(private http: HttpClient,
              private router: Router,
              private authService: AuthService) {
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

  @ViewChild('generalErrorToast') private generalErrorToast?: any;
  onSubmit() {
    this.http.post('api/users/login', this.loginForm.value).subscribe({
      next: _ => {
        this.authService.isAuthenticate = true;
        this.authService.fetchIsAdmin();
        this.router.navigate(['/']);
      },
      error: error => {
        if (error.status === 404 || error.status === 401)
          this.error = "Invalid email or password";
        if (error.status === 403)
          this.error = "The user is blocked";
        if (error.status == 400)
          this.generalErrorToast.visible = true;
      }
    });
  }
}
