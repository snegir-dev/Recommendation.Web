import {Component} from '@angular/core';
import {AbstractControl, FormControl, FormGroup, ValidationErrors, ValidatorFn, Validators} from "@angular/forms";
import {HttpClient} from "@angular/common/http";
import {Router} from "@angular/router";

@Component({
  selector: 'app-registration',
  templateUrl: './registration.component.html',
  styleUrls: ['./registration.component.sass']
})
export class RegistrationComponent {
  error?: string;

  constructor(private http: HttpClient, private router: Router) {
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
        Validators.minLength(3)
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

  onSubmit() {
    this.http.post('api/users/register', this.registrationForm.value).subscribe({
      next: _ => this.router.navigate(['/']),
      error: error => {
        if (error.status === 409) {
          this.error = "A user with the same username or email address already exists";
        }
      }
    });
  }
}
