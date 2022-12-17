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

  getByUserIdOrDefault(userId?: string | null): Observable<ReviewDisplayDto[]> {
    let route = '/get-by-user'
    if (userId)
      route = `/get-by-user/${userId}`
    return this.http.get<ReviewDisplayDto[]>(this.reviewPath + route);
  }

  getUpdated(reviewId: string): Observable<ReviewUpdateDto> {
    return this.http.get<ReviewUpdateDto>(this.reviewPath + `/get-updated-review/${reviewId}`)
  }

  create(review: any, userId?: string | null): Observable<any> {
    let route = ''
    if (userId)
      route = `/${userId}`
    return this.http.post(this.reviewPath + route, review);
  }

  update(review: any): Observable<void> {
    return this.http.put<void>(this.reviewPath, review);
  }

  delete(reviewId: string): Observable<any> {
    return this.http.delete(this.reviewPath + `/${reviewId}`);
  }
}
