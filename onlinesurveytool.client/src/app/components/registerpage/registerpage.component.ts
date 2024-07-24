                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                          import { Component } from '@angular/core';
import { AbstractControl, FormControl, FormGroup, ValidationErrors, ValidatorFn } from '@angular/forms';
import { Validators } from '@angular/forms';
import { UserService } from '../../services/user.service';
import { UserRegisterDTO } from '../../models/user.register.dto';

@Component({
  selector: 'register-component',
  templateUrl: './registerpage.component.html',
  styleUrls: ['../../styles/userForm.css'],
})
export class RegisterComponent {

  constructor(private userService: UserService) { }

  loginErrorText: string = '';
  emailErrorText: string = '';
  passwordErrorText: string = '';
  responseMessage: string = '';
  responseColor: string = 'red';
  loading: boolean = false;

  registerForm = new FormGroup({
    login: new FormControl('', [Validators.required, Validators.minLength(8), Validators.pattern(/^\S*$/)]),
    email: new FormControl('', [Validators.required, Validators.email]),
    password: new FormControl('', [Validators.required, Validators.minLength(8), this.passwordValidator()]),
    passwordRepeat: new FormControl('')
  });

  handleSubmit() {
    if (this.registerForm.valid) {
      if (this.registerForm.value.passwordRepeat !== this.registerForm.value.password) {
        this.passwordErrorText = "Passwords are not the same!";
      }

      this.loading = true;
      this.registerForm.disable();
      this.loginErrorText = "";
      this.emailErrorText = "";
      this.passwordErrorText = "";

      this.registerUser();

    }
    else {
      this.checkLogin();
      this.checkEmail();
      this.checkPassword();
    }
  }



  registerUser() {
    this.responseMessage = '';
    let registerDto: UserRegisterDTO = new UserRegisterDTO(
      this.registerForm.value.login,
      this.registerForm.value.email,
      this.registerForm.value.password);

    this.userService.registerUser(registerDto)
      .subscribe({
        next: (response: any) => this.handleRegisterResponse(response),
        error: (error: any) => this.handleError(error)
      })
  }

  handleRegisterResponse(response: any) {
    this.responseColor = 'lime';
    this.responseMessage = 'User registered succesfully! You can go back and log in now!';
    console.log(response);
  }

  handleError(error: any) {
    if (error.status == 409) {
      this.responseColor = 'red';
      this.responseMessage = 'This username is already used by someone else! Choose a different username!';
    }
    else {
      this.responseColor = 'red';
      this.responseMessage = 'Something went wrong! Try again later.'
      console.log(error);
    }

      this.loading = false;
      this.registerForm.enable();
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
    } else if (this.registerForm.value.passwordRepeat !== this.registerForm.value.password) {
        this.passwordErrorText = "Passwords are not the same!";
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

