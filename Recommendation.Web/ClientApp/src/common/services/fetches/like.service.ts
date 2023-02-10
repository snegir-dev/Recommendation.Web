import {HttpClient} from "@angular/common/http";
import {Injectable} from "@angular/core";
import {Observable} from "rxjs";
import { SetLikeModel } from "src/common/models/like/set.like.model";

@Injectable({
  providedIn: 'root'
})
export class LikeService {
  private baseRoute = 'api/likes';
  constructor(private httpClient: HttpClient) {
  }

  setLike(likeModel: SetLikeModel): Observable<void> {
    return this.httpClient.post<void>(this.baseRoute, likeModel);
  }
}
