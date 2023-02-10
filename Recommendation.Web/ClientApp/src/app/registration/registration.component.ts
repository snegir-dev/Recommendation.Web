import {Component, ViewChild} from '@angular/core';
import {AbstractControl, FormControl, FormGroup, ValidationErrors, ValidatorFn, Validators} from "@angular/forms";
import {HttpClient} from "@angular/common/http";
import {Router} from "@angular/router";
import {AuthService} from "../../common/services/auths/auth.service";

@Component({
  selector: 'app-registration',
  templateUrl: './registration.component.html',
  styleUrls: ['./registration.component.sass']
})
export class RegistrationComponent {
  error?: string;

  constructor(private http: HttpClient,
              private router: Router,
              private authService: AuthService) {
  }

  checkPasswords: ValidatorFn = (group: AbstractControl): ValidationErrors | null => {
    let pass = group.get('password')?.value;
    let confirmPass = group.get('passwordConfirmation')?.value;
    return pass === confirmPass ? null : {notSame: true};
  }

  registrationForm: FormGroup = new FormGroup({
    login: new FormControl('',
      [
        Validators.required,
        Validators.maxLength(100),
        Validators.minLength(5)
      ]),
    email: new FormControl('',
      [
        Validators.required,
        Validators.email
      ]),
    password: new FormControl('',
      [
        Validators.required,
        Validators.minLength(5)
      ]),
    passwordConfirmation: new FormControl('', [
      Validators.required
    ]),
    isRemember: new FormControl(false)
  }, {validators: this.checkPasswords});

  @ViewChild('generalErrorToast') private generalErrorToast?: any;

  onSubmit() {
    this.http.post('api/users/register', this.registrationForm.value).subscribe({
      next: _ => {
        this.authService.isAuthenticate = true;
        this.authService.fetchIsAdmin();
        this.router.navigate(['/'])
      },
      error: error => {
        if (error.status === 409)
          this.error = "A user with the same username or email address already exists";
        if (error.status == 400)
          this.generalErrorToast.visible = true;
      }
    });
  }
}
