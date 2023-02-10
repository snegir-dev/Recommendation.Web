import {Injectable} from "@angular/core";
import {ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot} from "@angular/router";
import {AuthService} from "../services/auths/auth.service";
import {map, Observable} from "rxjs";

@Injectable()
export class AuthGuard implements CanActivate {
  constructor(private authService: AuthService, private router: Router) {
  }

  canActivate(next: ActivatedRouteSnapshot, state: RouterStateSnapshot):
    Observable<boolean> | Promise<boolean> | boolean {
    return this.isSignedIn();
  }

  isSignedIn(): Observable<boolean> | Promise<boolean> | boolean {
    return this.authService.fetchIsSignedIn().pipe(
      map((isSignedIn) => {
        if (!isSignedIn) {
          this.router.navigate(['/login']);
          this.authService.isAuthenticate = false;
          return false;
        }
        this.authService.isAuthenticate = true;
        return true;
      }));
  }
}
