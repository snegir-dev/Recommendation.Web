import {Injectable} from "@angular/core";
import {HttpClient} from "@angular/common/http";
import {SetRatingModel} from "../../models/rating/setRatingModel";
import {Observable} from "rxjs";

@Injectable({
  providedIn: 'root'
})
export class RatingService {
  private baseRoute = 'api/rating';
  constructor(private httpClient: HttpClient) {
  }

  setRating(gradeModel: SetRatingModel): Observable<void> {
    return this.httpClient.post<void>(this.baseRoute, gradeModel);
  }
}
