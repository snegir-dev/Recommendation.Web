import {Injectable} from "@angular/core";
import {catchError, map, Observable, of} from "rxjs";
import {UserClaim} from "../../models/user/user.claim";
import {HttpClient} from "@angular/common/http";
import {ClaimNames} from "../../models/auth/claim.names";

@Injectable()
export class AuthService {
  constructor(private http: HttpClient) {
    this.fetchIsSignedIn().subscribe(isAuthenticate =>
      this.isAuthenticate = isAuthenticate);
    this.fetchIsAdmin();
  }

  private baseRoute: string = 'api/users';
  isAuthenticate: boolean = false;
  isAdmin: boolean = false;

  getClaims(): Observable<UserClaim[]> {
    return this.http.get<UserClaim[]>(this.baseRoute + '/get-claims');
  }

  getValueClaim(claimName: string): Observable<string> {
    return this.getClaims()
      .pipe(map(claims => this.claimsToObject(claims)[claimName]));
  }

  fetchIsAdmin() {
    this.getValueClaim(ClaimNames.role).subscribe(role => {
      if (role === 'Admin')
        this.isAdmin = true;
    });
  }

  fetchIsSignedIn(): Observable<boolean> {
    return this.getClaims().pipe(
      map((userClaims) => {
        return userClaims.length > 0;
      }),
      catchError((_) => {
        return of(false);
      }));
  };

  private claimsToObject(userClaims: UserClaim[]): any {
    return userClaims.reduce((accumulator, value) => {
      return {...accumulator, [value.type]: value.value};
    }, {});
  }
}
