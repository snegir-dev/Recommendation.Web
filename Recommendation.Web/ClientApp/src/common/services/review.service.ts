import {HttpClient, HttpParams} from "@angular/common/http";
import {Observable} from "rxjs";
import {Injectable} from "@angular/core";
import {ReviewInfo} from "../models/Review/ReviewInfo";

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

  getReview(params: any): Observable<ReviewInfo> {
    return this.http.get<ReviewInfo>(this.reviewPath, {params});
  }
}
