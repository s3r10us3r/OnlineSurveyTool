import { Component } from '@angular/core';
import { AbstractControl, FormControl, FormGroup, ValidationErrors, ValidatorFn } from '@angular/forms'
import { Validators } from '@angular/forms'


@Component({
  selector: 'register-component',
  templateUrl: './registerpage.component.html',
  styleUrls: ['../../styles/userForm.css'],
})
export class RegisterComponent {
  loginErrorText: string = '';
  emailErrorText: string = '';
  passwordErrorText: string = '';

  registerForm = new FormGroup({
    login: new FormControl('', [Validators.required, Validators.minLength(8)]),
    email: new FormControl('', [Validators.required, Validators.email]),
    password: new FormControl('', [Validators.required, Validators.minLength(8), this.passwordValidator()])
  });

  handleSubmit() {
    if (this.registerForm.valid) {
      this.loginErrorText = "";
      this.emailErrorText = "";
      this.passwordErrorText = "";

      this.registerForm.disable();
    }
    else {   
      this.checkLogin();
      this.checkEmail();
      this.checkPassword();
    }
  }

  checkLogin(): void {
      const loginControl = this.registerForm.get('login');
      this.loginErrorText = "";

      if (loginControl?.errors) {
        if (loginControl.errors['required']) {
          this.loginErrorText = "Username is required";
        } else if (loginControl.errors['minlength']) {
          this.loginErrorText = 'Username must be at least 8 characters long';
        }
      }
  }

  checkEmail(): void {
      const emailControl = this.registerForm.get('email');
      this.emailErrorText = "";

      if (emailControl?.errors) {
        if (emailControl.errors['required']) {
          this.emailErrorText = "Email is required";
        } else if (emailControl.errors['email']) {
          this.emailErrorText = "Email is not valid";
        }
      }
  }

  checkPassword(): void {
      const passwordControl = this.registerForm.get('password');
      this.passwordErrorText = ""

      if (passwordControl?.errors) {
        if (passwordControl.errors['required']) {
          this.passwordErrorText = 'Password is required';
        } else if (passwordControl.errors['minlength']) {
          this.passwordErrorText = 'Password must be at least 8 characters long!'
        } else if (passwordControl.errors['passwordStrength']) {
          this.passwordErrorText = 'Password must contain at least one lowercase and uppercase letter, one digit and one non-alphanumeric character.'
        }
      }

  }

  passwordValidator(): ValidatorFn {
    return (control: AbstractControl): ValidationErrors | null => {
      const value = control.value;
      if (!value) {
        return null;
      }

      const hasUpperCase = /[A-Z]/.test(value);
      const hasLowerCase = /[a-z]/.test(value);
      const hasDigit = /\d/.test(value);
      const hasNonAlphanumeric = /\W/.test(value);

      const valid = hasUpperCase && hasLowerCase && hasDigit && hasNonAlphanumeric;
      return !valid ? { passwordStrength: true } : null;
    }
  }

}

