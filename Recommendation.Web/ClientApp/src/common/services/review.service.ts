import {HttpClient, HttpParams} from "@angular/common/http";
import {Observable} from "rxjs";
import {Injectable} from "@angular/core";
import {ReviewInfo} from "../models/Review/ReviewInfo";
import {ReviewModel} from "../models/Review/ReviewModel";

@Injectable({
  providedIn: 'root'
})
export class ReviewService {
  private reviewPath: string = 'api/reviews'

  constructor(private http: HttpClient) {
  }

  get(reviewId: string): Observable<ReviewModel> {
    return this.http.get<ReviewModel>(this.reviewPath + `/${reviewId}`);
  }

  getByParams(params: any): Observable<ReviewInfo> {
    return this.http.get<ReviewInfo>(this.reviewPath, {params});
  }

  create(review: any): Observable<any> {
    return this.http.post(this.reviewPath, review);
  }
}
