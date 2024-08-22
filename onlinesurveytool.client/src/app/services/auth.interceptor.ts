import {Injectable} from "@angular/core";
import {
  HttpErrorResponse,
  HttpEvent,
  HttpHandler,
  HttpInterceptor,
  HttpRequest,
  HttpResponse
} from "@angular/common/http";
import {catchError, EMPTY, Observable, switchMap} from "rxjs";
import {AuthService} from "./auth.service";
import {Router} from "@angular/router";

@Injectable()
export class AuthInterceptor implements HttpInterceptor {

  constructor(private authService: AuthService, private router: Router) {}

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    if (req.url.includes('Auth'))
      return next.handle(req);

    if (this.authService.isAccessValid()) {
      //there is a null check in the isAccessValid method
      req = this.addToken(req, this.authService.getAccessToken()!);
      return next.handle(req);
    }

    this.authService.clearAccessToken();

    return this.authService.refreshAccess().pipe(
      switchMap(() => {
        req = this.addToken(req, this.authService.getAccessToken()!);
        return next.handle(req);
      }),
      catchError((err) => {
        console.error(err);
        if (err instanceof HttpErrorResponse && err.status === 401) {
          this.router.navigate(['/login']);
        } else {
          this.router.navigate(['/error']);
        }
        return EMPTY;
      })
    );
  }

  private addToken(req: HttpRequest<any>, token: string): HttpRequest<any> {
    return req.clone({
      setHeaders: {
        Authorization: `Bearer ${token}`
      }
    });
  }
}
