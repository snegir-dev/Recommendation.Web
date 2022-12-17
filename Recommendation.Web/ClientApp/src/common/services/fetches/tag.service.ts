import {Injectable} from "@angular/core";
import {HttpClient} from "@angular/common/http";
import {map, Observable} from "rxjs";

@Injectable({
  providedIn: 'root'
})
export class TagService {
  constructor(private http: HttpClient) {
  }

  private baseRoute: string = 'api/tags';

  getTags(): Observable<string[]> {
    return this.http.get<string[]>(this.baseRoute).pipe(
      map(tags => (<any>tags).map((tag: any) => tag['tag']))
    );
  }
}
