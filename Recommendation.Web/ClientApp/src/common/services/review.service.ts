import {HttpClient, HttpParams} from "@angular/common/http";
import {Observable} from "rxjs";
import {Injectable} from "@angular/core";
import {ReviewInfo} from "../models/Review/ReviewInfo";
import {ReviewModel} from "../models/Review/ReviewModel";
import {ReviewDisplayDto} from "../models/Review/ReviewDisplayDto";
import {ReviewUpdateDto} from "../models/Review/ReviewUpdateDto";

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

  getByUserId(userId: string): Observable<ReviewDisplayDto[]> {
    return this.http.get<ReviewDisplayDto[]>(this.reviewPath + `/get-by-user-id/${userId}`);
  }

  getUpdated(reviewId: string): Observable<ReviewUpdateDto> {
    return this.http.get<ReviewUpdateDto>(this.reviewPath + `/get-updated-review/${reviewId}`)
  }

  create(review: any): Observable<any> {
    return this.http.post(this.reviewPath, review);
  }

  update(review: any): Observable<void> {
    return this.http.put<void>(this.reviewPath, review);
  }

  delete(reviewId: string): Observable<any> {
    return this.http.delete(this.reviewPath + `/${reviewId}`);
  }
}
