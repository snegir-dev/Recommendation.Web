﻿import {Injectable, OnInit} from "@angular/core";
import {HttpClient} from "@angular/common/http";
import {catchError, map, Observable, of} from "rxjs";
import {UserClaim} from "../../models/user/user.claim";
import {UserModel} from "../../models/user/user.model";
import {UserInfo} from "../../models/user/user.info";

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

  getUserInfo(userId?: string): Observable<UserInfo> {
    let route = '/get-user-info';
    if (userId)
      route = route + `/${userId}`;
    return this.http.get<UserInfo>(this.baseRoute + route);
  }

  logout(): Observable<void> {
    return this.http.post<void>(this.baseRoute + '/logout', null);
  }

  blockUser(userId: string): Observable<void> {
    return this.http.post<void>(this.baseRoute + `/block/${userId}`, {});
  }

  unblockUser(userId: string): Observable<void> {
    return this.http.post<void>(this.baseRoute + `/unblock/${userId}`, {});
  }

  deleteUser(userId: string): Observable<void> {
    return this.http.delete<void>(this.baseRoute + `/delete/${userId}`);
  }

  setUserRole(userId: string, roleName: string): Observable<void> {
    return this.http.post<void>(this.baseRoute + '/set-role',
      {userId: userId, roleName: roleName});
  }
}
