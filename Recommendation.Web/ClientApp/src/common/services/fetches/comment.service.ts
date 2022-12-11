import {Injectable} from "@angular/core";
import {HttpClient} from "@angular/common/http";
import {Observable} from "rxjs";
import {CommentModel} from "../../models/comment/comment.model";
import {FormGroup} from "@angular/forms";

@Injectable({
  providedIn: 'root'
})
export class CommentService {
  private baseRoute = 'api/comments';

  constructor(private httpClient: HttpClient) {
  }

  getAll(reviewId: string): Observable<CommentModel[]> {
    return this.httpClient.get<CommentModel[]>(this.baseRoute,
      {params: {reviewId: reviewId}});
  }

  post(commentForm: FormGroup): Observable<any> {
    return this.httpClient.post(this.baseRoute, commentForm.value);
  }
}
