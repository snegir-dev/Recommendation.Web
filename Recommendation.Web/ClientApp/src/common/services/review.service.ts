import {HttpClient, HttpParams} from "@angular/common/http";
import {Observable} from "rxjs";
import {Injectable} from "@angular/core";
import {ReviewInfo} from "../models/Review/ReviewInfo";
import {ReviewModel} from "../models/Review/ReviewModel";
import {ReviewDto} from "../models/Review/ReviewDto";

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

  getByUserId(userId: string): Observable<ReviewDto[]> {
    return this.http.get<ReviewDto[]>(this.reviewPath + `/get-by-user-id/${userId}`);
  }

  create(review: any): Observable<any> {
    return this.http.post(this.reviewPath, review);
  }

  delete(reviewId: string): Observable<any> {
    return this.http.delete(this.reviewPath + `/${reviewId}`);
  }
}
