import {Injectable} from "@angular/core";
import {Observable} from "rxjs";
import {HttpClient} from "@angular/common/http";

@Injectable({
  providedIn: 'root'
})
export class ImageService {
  constructor(private httpClient: HttpClient) {
  }

  getImageBlob(url: string): Observable<Blob> {
    return this.httpClient.get(url, {responseType: 'blob'});
  }
}
