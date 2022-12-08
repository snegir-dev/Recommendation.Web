import {HttpClient} from "@angular/common/http";
import {Observable} from "rxjs";
import {Injectable} from "@angular/core";

@Injectable({
  providedIn: 'root'
})
export class ReviewService {
  private reviewPath: string = 'api/reviews'

  constructor(private http: HttpClient) {
  }

  createReview(review: any): Observable<any> {
    return this.http.post(this.reviewPath, review);
  }
}
