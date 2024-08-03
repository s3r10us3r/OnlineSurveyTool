import {inject, Injectable} from "@angular/core";
import {HttpClient} from "@angular/common/http";
import {API_URL} from '../constants';
import {catchError, map, Observable, of} from "rxjs";
import {jwtDecode} from "jwt-decode";
import {ActivatedRouteSnapshot, CanActivateFn, Router, RouterStateSnapshot} from "@angular/router";

export interface LoginResponse {
  accessToken: string;
  accessTokenExpirationDateTime: string;
}

export interface RefreshResponse {
  accessToken: string;
  accessTokenExpirationDateTime: string;
}

@Injectable()
export class AuthService {

  private static accessToken: string | undefined = undefined;
  private static accessTokenExpiration: Date | undefined = undefined;

  constructor(private http: HttpClient) {
  }

  public login(login:string, password:string): Observable<LoginResponse> {
    return this.http.post<LoginResponse>(`${API_URL}/Auth/login`, {login, password});
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

  public refreshAccess(): Observable<any> {
    return this.http.get(`${API_URL}/Auth/refresh`, { withCredentials: true });
  }

  public clearAccessToken() {
    AuthService.accessToken = undefined;
    AuthService.accessTokenExpiration = undefined;
  }

  public isLoggedIn(): Observable<boolean> {
    if (this.isAccessValid()) {
      console.debug('access valid');
      return of(true);
    }

    return this.refreshAccess().pipe(
      map(res => {
        console.debug('refresh response correct', res);
        return true;
      }),
      catchError(err => {
        console.debug('refresh response not correct', err);
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

  public setSession(token: string, expiration: string) : void {
    AuthService.accessToken = token;
    AuthService.accessTokenExpiration = new Date(expiration);
    console.debug('Access token set', AuthService.accessToken);
    console.debug('Expiration date set', AuthService.accessTokenExpiration);
    console.debug('Time now', new Date());
    console.debug('Comparison', AuthService.accessTokenExpiration > new Date());
  }

  private getAccessExpirationDateTime() : Date | undefined {
    return AuthService.accessTokenExpiration;
  }
}

export const canUseRoute: CanActivateFn = (
  route: ActivatedRouteSnapshot,
  state: RouterStateSnapshot,
) : Observable<boolean> => {
  let authService = inject(AuthService);
  let router = inject(Router);

  return authService.isLoggedIn().pipe(
    map(isLoggedIn => {
      if (isLoggedIn) {
        console.error('logged in');
        return true;
      } else {
        console.error('not logged in');
        router.navigate(['/login']);
        return false;
      }
    }),
    catchError(err => {
      console.error('Error during authentication guard check', err);
      router.navigate(['/login']);
      return of(false);
    })
  )
}
