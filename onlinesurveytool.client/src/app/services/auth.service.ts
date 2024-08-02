import {inject, Injectable} from "@angular/core";
import {HttpClient} from "@angular/common/http";
import {API_URL} from '../constants';
import {tap} from 'rxjs/operators'
import {catchError, map, Observable, of} from "rxjs";
import {jwtDecode} from "jwt-decode";
import {ActivatedRouteSnapshot, CanActivateFn, Router, RouterStateSnapshot} from "@angular/router";

export interface LoginResponse {
  accessToken: string;
  accessTokenExpirationDateTime: Date;
}

export interface RefreshResponse {
  accessToken: string;
  accessTokenExpirationDateTime: Date;
}

@Injectable()
export class AuthService {

  private static accessToken: string | undefined = undefined;
  private static accessTokenExpiration: Date | undefined = undefined;

  constructor(private http: HttpClient) {
  }

  public login(login:string, password:string): Observable<LoginResponse> {
    return this.http.post<LoginResponse>(`${API_URL}/Auth/login`, {login, password})
      .pipe(
        tap(res => this.setSession(res)),
      );
  }

  public logout() : void {
    AuthService.accessToken = undefined;
    AuthService.accessTokenExpiration = undefined;
  }

  public isAccessValid() : boolean {
    const token = this.getAccessToken();
    const expirationDate = this.getAccessExpirationDateTime();
    const dateNow = new Date();

    return token != null && expirationDate != null && expirationDate > dateNow;
  }

  public getAccessToken(): string | undefined {
    return AuthService.accessToken;
  }

  public refreshAccess(): Observable<RefreshResponse> {
    return this.http.get<RefreshResponse>(`${API_URL}/Auth/refresh`)
      .pipe(
        tap(res => this.setAccessToken(res.accessToken, res.accessTokenExpirationDateTime)),
      );
  }

  public clearAccessToken() {
    AuthService.accessToken = undefined;
    AuthService.accessTokenExpiration = undefined;
  }

  public isLoggedIn(): Observable<boolean> {
    if (this.isAccessValid()) {
      return of(true);
    }

    return this.refreshAccess().pipe(
      map(res => {
        return true;
      }),
      catchError(err => {
        this.clearAccessToken();
        return of(false);
      })
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

  private setSession(authResult: LoginResponse) : void {
    AuthService.accessToken = authResult.accessToken;
    AuthService.accessTokenExpiration = authResult.accessTokenExpirationDateTime;
  }

  private setAccessToken(token: string, expiration: Date) : void {
    AuthService.accessToken = token;
    AuthService.accessTokenExpiration = expiration;
  }

  private getAccessExpirationDateTime() : Date | undefined {
    return AuthService.accessTokenExpiration;
  }
}

export const canUseRoute: CanActivateFn = (
  route: ActivatedRouteSnapshot,
  state: RouterStateSnapshot,
) => {
  if (inject(AuthService).isLoggedIn()) {
    return true;
  } else {
    inject(Router).navigate(['/login']);
    return false;
  }
}
