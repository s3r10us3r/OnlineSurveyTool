import { Component } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms'
import { UserService } from '../../services/user.service';
import { UserLoginDTO } from '../../../dtos/user.login.dto';

@Component({
  selector: 'login-component',
  templateUrl: 'loginpage.component.html',
  styleUrl: '../../styles/userForm.css',
})
export class LoginComponent {

  constructor(private userService: UserService) { }

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

      let loginDTO : UserLoginDTO = new UserLoginDTO(
        loginString,
        passwordString
      );

      this.userService.loginUser(loginDTO)
        .subscribe({
          next: (response: any) => this.handleLoginResponse(response),
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

  //TODO: HANDLE LOGGING IN
  handleLoginResponse(response: any): void {
    console.log("NOT IMPLEMENTED YET BUT LOGGED IN!");
    console.log(response);
  }

  handleErrorResponse(error: any): void {
    this.responseMessage = "Invalid credentials!"
  }

}
