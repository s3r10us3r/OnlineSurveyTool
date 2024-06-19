import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { API_URL } from '../constants';
import { Observable } from 'rxjs';
import { UserRegisterDTO } from '../../dtos/user.register.dto';
import { UserLoginDTO } from '../../dtos/user.login.dto';
import { TokenResponseDTO } from '../../dtos/token.response.dto';

@Injectable({
  providedIn: 'root'
})
export class UserService {
  private apiUrl = API_URL + '/Auth'

  constructor(private http: HttpClient) { }

  registerUser(data: UserRegisterDTO): Observable<UserRegisterDTO> {
    return this.http.post<any>(`${this.apiUrl}/register`, data);
  }

  loginUser(data: UserLoginDTO): Observable<TokenResponseDTO> {
    return this.http.post<any>(`${this.apiUrl}/login`, data);
  }
}
