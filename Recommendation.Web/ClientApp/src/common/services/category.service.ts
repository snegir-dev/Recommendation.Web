import {Injectable} from "@angular/core";
import {HttpClient} from "@angular/common/http";
import {Observable} from "rxjs";

@Injectable({
  providedIn: 'root'
})
export class CategoryService {
  baseRoute = 'api/categories';

  constructor(private http: HttpClient) {
  }

  getAllCategories() : Observable<any> {
    return this.http.get(this.baseRoute);
  }
}
