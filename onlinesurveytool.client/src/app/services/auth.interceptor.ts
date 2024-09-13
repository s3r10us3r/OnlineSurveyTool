import { Injectable } from "@angular/core";
import {
  HttpErrorResponse,
  HttpEvent,
  HttpHandler,
  HttpInterceptor,
  HttpRequest,
} from "@angular/common/http";
import { BehaviorSubject, Observable, throwError } from "rxjs";
import { catchError, filter, switchMap, take } from "rxjs/operators";
import { AuthService } from "./auth.service";
import { Router } from "@angular/router";

@Injectable()
export class AuthInterceptor implements HttpInterceptor {

  private isRefreshing = false;
  private refreshTokenSubject: BehaviorSubject<string | null> = new BehaviorSubject<string | null>(null);

  constructor(private authService: AuthService, private router: Router) {}

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    console.debug('Request intercepted:', req.url);

    // Skip interception for Auth endpoints
    if (req.url.includes('Auth') || req.url.includes('Survey/get') || req.url.includes('Answer')) {
      console.debug('let go')
      return next.handle(req);
    }

    // If the token is valid, add it to the request
    if (this.authService.isAccessValid()) {
      req = this.addToken(req, this.authService.getAccessToken()!);
      return next.handle(req);
    }

    // If the token is not valid, try to refresh it
    return this.handleTokenRefresh(req, next);
  }

  private handleTokenRefresh(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    if (!this.isRefreshing) {
      this.isRefreshing = true;
      this.refreshTokenSubject.next(null);

      return this.authService.refreshAccess().pipe(
        switchMap((newToken: any) => {
          this.isRefreshing = false;
          this.authService.setSession(newToken.accessToken, newToken.accessTokenExpirationDateTime);
          this.refreshTokenSubject.next(newToken.accessToken);
          req = this.addToken(req, newToken.accessToken);
          return next.handle(req);
        }),
        catchError((err: HttpErrorResponse) => {
          this.isRefreshing = false;
          this.authService.clearAccessToken();
          this.router.navigate(['/login']);
          return throwError(() => err);
        })
      );
    } else {
      // Wait for the token refresh to complete and then retry the request
      return this.refreshTokenSubject.pipe(
        filter(token => token !== null),
        take(1),
        switchMap(token => {
          req = this.addToken(req, token!);
          return next.handle(req);
        })
      );
    }
  }

  // Helper method to add the token to the request headers
  private addToken(req: HttpRequest<any>, token: string): HttpRequest<any> {
    return req.clone({
      setHeaders: {
        Authorization: `Bearer ${token}`
      }
    });
  }
}
