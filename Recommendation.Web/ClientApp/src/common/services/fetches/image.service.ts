import {Injectable} from "@angular/core";
import {BehaviorSubject, forkJoin, map, mergeAll, Observable, of, Subject, zip} from "rxjs";
import {HttpClient} from "@angular/common/http";
import {combineLatest, mergeMap} from "rxjs/operators";
import {ImageMetadata} from "../../models/image/image.metadata";

@Injectable({
  providedIn: 'root'
})
export class ImageService {
  constructor(private httpClient: HttpClient) {
  }

  getImageBlob(url: string): Observable<Blob> {
    return this.httpClient.get(url, {responseType: 'blob'});
  }

  getImageBlobFromImageMetadata(imageMetadata: ImageMetadata): Observable<{ blob: Blob, fileName: string }> {
    return this.httpClient.get(imageMetadata.url, {responseType: 'blob'}).pipe(
      map(blob => ({blob: blob, fileName: imageMetadata.name}))
    );
  }
}
