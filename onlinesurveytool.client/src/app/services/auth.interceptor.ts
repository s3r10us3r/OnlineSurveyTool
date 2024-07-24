import {Injectable} from "@angular/core";
import {HttpErrorResponse, HttpEvent, HttpHandler, HttpInterceptor, HttpRequest} from "@angular/common/http";
import {catchError, Observable, switchMap, take, throwError} from "rxjs";
import {AuthService} from "./auth.service";
import {Router} from "@angular/router";

@Injectable()
export class AuthInterceptor implements HttpInterceptor {

  constructor(private authService: AuthService, private router: Router) {}

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    if (this.authService.isAccessValid()) {
      //there is a null check in the isAccessValid method
      req = this.addToken(req, this.authService.getAccessToken()!);
      return next.handle(req);
    }

    if (this.authService.isLoggedIn()) {
      this.authService.refreshAccess().pipe(
        take(1),
        switchMap(response => {
          console.log('Access token refreshed response:', response);
          req = this.addToken(req, this.authService.getAccessToken()!);
          return next.handle(req);
        }),
        catchError(err => this.handleRefreshError(err))
      );
    } else {
      return this.handleLoggedOff();
    }
    return next.handle(req);
  }

  private addToken(req: HttpRequest<any>, token: string): HttpRequest<any> {
    return req.clone({
      setHeaders: {
        Authorization: `Bearer ${token}`
      }
    });
  }

  private handleRefreshError(error: any): Observable<any>  {
    this.authService.logout();
    if (error instanceof HttpErrorResponse && error.status === 401) {
      this.router.navigate(['/login']);
    } else {
      console.error(error);
      this.router.navigate(['/error']);
    }

    return throwError(() => {
      return Error('Could not refresh access token ' + error.message);
    });
  }

  private handleLoggedOff(): Observable<any> {
    this.authService.logout();
    this.router.navigate(['/login']);
    return throwError(() => {
      new Error('User is logged out.')
    });
  }

}
