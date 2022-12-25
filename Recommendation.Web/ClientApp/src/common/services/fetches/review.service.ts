import {HttpClient, HttpParams} from "@angular/common/http";
import {Observable} from "rxjs";
import {Injectable} from "@angular/core";
import { ReviewModel } from "src/common/models/review/review.model";
import {ReviewInfo} from "../../models/review/review.info";
import {ReviewCardDto} from "../../models/review/reviewCardDto";
import {ReviewUpdateDto} from "../../models/review/review.update.dto";

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

  getByUserIdOrDefault(userId?: string | null): Observable<ReviewCardDto[]> {
    let route = '/get-by-user'
    if (userId)
      route = `/get-by-user/${userId}`
    return this.http.get<ReviewCardDto[]>(this.reviewPath + route);
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
