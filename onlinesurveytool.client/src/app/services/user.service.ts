import {Injectable} from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {API_URL} from '../constants';
import {map, Observable} from 'rxjs';
import {UserRegisterDTO} from '../models/user.register.dto';
import {UserLoginDTO} from '../models/user.login.dto';

@Injectable()
export class UserService {
  private apiUrl = API_URL + '/Auth'

  constructor(private http: HttpClient) { }

  registerUser(data: UserRegisterDTO): Observable<UserRegisterDTO> {
    return this.http.post<any>(`${this.apiUrl}/register`, data);
  }
}
