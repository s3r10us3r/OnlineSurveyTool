import { Component } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms'
import { UserService } from '../../services/user.service';
import { LoginResponse } from "../../models/login.response";
import {AuthService} from "../../services/auth.service";
import {Router} from "@angular/router";
import {HttpErrorResponse} from "@angular/common/http";

@Component({
  selector: 'login-component',
  templateUrl: 'loginpage.component.html',
  styleUrl: '../../styles/userForm.css',
})
export class LoginComponent {

  constructor(private userService: UserService, private authService: AuthService, private router: Router) { }

  responseMessage : string = '';
  isLoading : boolean = false;

  loginForm = new FormGroup({
    login: new FormControl('', [Validators.required]),
    password: new FormControl('', [Validators.required])
  });

  handleSubmit() {
    this.responseMessage = '';
    this.isLoading = true;
    const loginControl = this.loginForm.get('login');
    const passwordControl = this.loginForm.get('password');

    if (!loginControl || !passwordControl) {
      throw new Error('Controls are null!');
    }

    if (this.loginForm.valid) {
      const loginString = this.loginForm.value.login;
      const passwordString = this.loginForm.value.password;

      if (typeof loginString !== 'string' || typeof  passwordString !== 'string') {
        throw new Error("Login and password cannot be null!")
      }

      this.authService.login(loginString, passwordString)
        .subscribe({
          next: (response: LoginResponse) => this.handleLoginResponse(response),
          error: (error: any) => this.handleErrorResponse(error)
        });
    }
    else {
      if (loginControl?.errors && loginControl.errors['required']) {
        this.responseMessage = 'Username is required!';
        return;
      }

      if (passwordControl?.errors && passwordControl?.errors['required']){
        this.responseMessage = 'Password is required!';
        return;
      }
    }

    this.isLoading = false;
  }

  handleLoginResponse(response: LoginResponse): void {
    this.router.navigate(['/main']);
  }

  handleErrorResponse(error: any): void {
    if (error instanceof HttpErrorResponse && error.status === 401) {
      this.responseMessage = "Invalid credentials!"
    } else {
      console.error(error);
      this.router.navigate(['/error'])
    }
  }

}
