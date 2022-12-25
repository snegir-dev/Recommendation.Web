import {Injectable} from "@angular/core";
import {HttpClient} from "@angular/common/http";
import {Observable} from "rxjs";
import {SetRatingModel} from "../../models/rating/set.rating.model";

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
