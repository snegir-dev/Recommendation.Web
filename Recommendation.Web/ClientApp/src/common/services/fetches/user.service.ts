import {Injectable, OnInit} from "@angular/core";
import {HttpClient} from "@angular/common/http";
import {catchError, map, Observable, of} from "rxjs";
import {UserClaim} from "../../models/user/user.claim";

@Injectable({
  providedIn: 'root'
})
export class UserService {
  constructor(private http: HttpClient) {
  }

  private baseRoute = 'api/users';

  logout(): Observable<void> {
    return this.http.post<void>(this.baseRoute + '/logout', null);
  }
}
