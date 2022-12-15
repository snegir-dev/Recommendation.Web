import {Injectable} from "@angular/core";
import {HttpClient} from "@angular/common/http";
import {Observable} from "rxjs";

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
