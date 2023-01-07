import {forwardRef, Inject, Injectable, Injector} from "@angular/core";
import {
  HttpClient,
  HttpErrorResponse,
  HttpEvent,
  HttpHandler,
  HttpInterceptor,
  HttpRequest
} from "@angular/common/http";
import {Router} from "@angular/router";
import {Observable, tap} from "rxjs";
import {AuthService} from "../services/auths/auth.service";
import {UserService} from "../services/fetches/user.service";

@Injectable()
export class ErrorInterceptor implements HttpInterceptor {
  constructor(private router: Router,
              private authService: AuthService) {
  }

  intercept(httpRequest: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    return next.handle(httpRequest).pipe(tap({
      error: err => {
        this.authService.fetchIsSignedIn()
          .subscribe(value => this.authService.isAuthenticate = value);

        if (err instanceof HttpErrorResponse) {
          if (err.status === 401)
            this.router.navigate(['/login']);
          else if (err.status === 403)
            this.router.navigate(['/access-denied']);
          else if (err.status === 404)
            this.noFoundErrorHandle(httpRequest);

          return;
        }
      }
    }));
  }

  private noFoundErrorHandle(httpRequest: HttpRequest<any>) {
    if (!httpRequest.url.includes('login') && !httpRequest.url.includes('registration')) {
      this.router.navigate(['/not-found'])
    }
  }
}
