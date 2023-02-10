import {Injectable} from "@angular/core";
import {HttpClient} from "@angular/common/http";
import {Observable} from "rxjs";

@Injectable({
  providedIn: 'root'
})
export class CompositionService {
  constructor(private httpClient: HttpClient) {
  }

  private readonly baseRoute: string = 'api/compositions';

  getAllComposition(): Observable<string[]> {
    return this.httpClient.get<string[]>(this.baseRoute);
  }
}
