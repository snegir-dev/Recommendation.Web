import {Injectable, OnInit} from "@angular/core";
import {HttpClient} from "@angular/common/http";
import {catchError, map, Observable, of} from "rxjs";
import {UserClaim} from "../../models/user/user.claim";
import {UserModel} from "../../models/user/user.model";

@Injectable({
  providedIn: 'root'
})
export class UserService {
  constructor(private http: HttpClient) {
  }

  private baseRoute = 'api/users';

  getUsers(): Observable<UserModel[]> {
    return this.http.get<UserModel[]>(this.baseRoute).pipe(
      map(usersVm => (<any>usersVm)['users'])
    );
  }

  logout(): Observable<void> {
    return this.http.post<void>(this.baseRoute + '/logout', null);
  }
}
