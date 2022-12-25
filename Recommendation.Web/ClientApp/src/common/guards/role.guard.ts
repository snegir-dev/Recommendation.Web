import {Injectable} from "@angular/core";
import {ActivatedRoute, ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot} from "@angular/router";
import {AuthService} from "../services/auths/auth.service";
import {map, Observable} from "rxjs";
import {ClaimNames} from "../models/auth/claim.names";

@Injectable()
export class RoleGuard implements CanActivate {
  constructor(private authService: AuthService,
              private router: Router) {
  }

  canActivate(next: ActivatedRouteSnapshot, state: RouterStateSnapshot) {
    return this.isContainsRole(next);
  }

  isContainsRole(route: ActivatedRouteSnapshot): Observable<boolean> {
    return this.authService.getClaims().pipe(
      map(claims => {
        let claim = claims.filter(claim => claim.type === ClaimNames.role)[0];
        if (claim !== undefined && route.data.roles.map((role: any) => role === claim.value))
          return true;

        this.router.navigate(['/access-denied']);
        return false;
      })
    );
  }
}
