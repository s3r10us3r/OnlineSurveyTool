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

  responseMessage = '';

  loginForm = new FormGroup({
    login: new FormControl('', [Validators.required]),
    password: new FormControl('', [Validators.required])
  });

  handleSubmit() {
    this.responseMessage = '';

    if (this.loginForm.valid) {

    } else if(this.)
  }
}
