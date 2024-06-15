import { Component } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms'

@Component({
  selector: 'login-component',
  templateUrl: 'loginpage.component.html',
  styleUrl: 'loginpage.component.css',
})
export class LoginComponent {
  loginForm = new FormGroup({
    login: new FormControl(''),
    password: new FormControl('')
  });
}
