import {inject, Injectable} from "@angular/core";
import {HttpClient} from "@angular/common/http";
import {API_URL} from '../constants';
import {shareReplay, tap} from 'rxjs/operators'
import {Observable} from "rxjs";
import {jwtDecode} from "jwt-decode";
import {ActivatedRouteSnapshot, CanActivateFn, Router, RouterStateSnapshot} from "@angular/router";

interface LoginResponse {
  accessToken: string;
  accessExpirationDateTime: Date;
  refreshToken: string;
  refreshExpirationDateTime: Date;
}

interface RefreshResponse {
  accessToken: string;
  accessExpirationDateTime: Date;
}

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  constructor(private http: HttpClient) {
  }

  public login(login:string, password:string): Observable<LoginResponse> {
    return this.http.post<LoginResponse>(`${API_URL}/Auth/login`, {login, password})
      .pipe(
        tap(res => this.setSession(res)),
        shareReplay()
      )
  }

  public logout() {
    localStorage.removeItem('accToken');
    localStorage.removeItem('accExpiresAt');
    localStorage.removeItem('rToken');
    localStorage.removeItem('rExpiresAt');
  }

  public isAccessValid() : boolean {
    const token = this.getAccessToken();
    const expirationDate = this.getAccessExpirationDateTime();
    const dateNow = new Date();

    return token != null && expirationDate != null && expirationDate > dateNow;
  }

  public isLoggedIn() : boolean {
    const token = this.getRefreshToken();
    const expirationDate = this.getRefreshExpirationDateTime();
    const dateNow = new Date();

    return token != null && expirationDate != null && expirationDate > dateNow;
  }


  public getAccessToken(): string | null {
    return localStorage.getItem('accToken');
  }

  public getRefreshToken(): string | null {
    return localStorage.getItem('rToken');
  }

  public refreshAccess(): Observable<RefreshResponse> {
    const refreshToken = localStorage.getItem('rToken');

    return this.http.post<RefreshResponse>(`${API_URL}/Auth/refresh`, {refreshToken})
      .pipe(
        tap(res => this.setAccessToken(res)),
        shareReplay()
      );
  }

  public getUserName(): string | null {
    const token = this.getAccessToken();

    if (token) {
      try {
        const decodeToken: any = jwtDecode(token);
        if (decodeToken && decodeToken.sub) {
          return decodeToken.sub;
        }
        return null;
      } catch (error) {
        console.error('Error decoding token', error);
        return null;
      }
    }
    return null;
  }

  private setSession(authResult: LoginResponse) {
    localStorage.setItem('accToken', authResult.accessToken);
    localStorage.setItem('accExpiresAt', authResult.accessExpirationDateTime.toString());
    localStorage.setItem('rToken', authResult.refreshToken);
    localStorage.setItem('rExpiresAt', authResult.refreshExpirationDateTime.toString());
  }

  private setAccessToken(refResult: RefreshResponse) {
    localStorage.setItem('accToken', refResult.accessToken);
    localStorage.setItem('accExpiresAt', refResult.accessExpirationDateTime.toString());
  }

  private getAccessExpirationDateTime() : Date | null {
    const expirationDateString = localStorage.getItem('accExpiresAt');
    return expirationDateString ? new Date(expirationDateString) : null;
  }

  private getRefreshExpirationDateTime() : Date | null {
    const expirationDateString = localStorage.getItem('rExpiresAt');
    return expirationDateString ? new Date(expirationDateString) : null;
  }
}

export const canUseRoute: CanActivateFn = (
  route: ActivatedRouteSnapshot,
  state: RouterStateSnapshot,
) => {
  if (inject(AuthService).isAccessValid()) {
    return true;
  } else {
    inject(Router).navigate(['/login']);
    return false;
  }
}
