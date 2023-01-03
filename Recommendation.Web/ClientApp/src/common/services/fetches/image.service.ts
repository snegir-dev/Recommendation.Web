import {Injectable} from "@angular/core";
import {map, Observable} from "rxjs";
import {HttpClient} from "@angular/common/http";
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

  async getBase64Image(url: string): Promise<string> {
    return new Promise(async (resolve, reject) => {
      const response = await fetch(url);
      const blob = await response.blob();
      const reader = new FileReader();

      reader.onload = function () {
        resolve(reader.result as string);
      };
      reader.onerror = function () {
        reject(reader.error);
      };

      reader.readAsDataURL(blob);
    });
  }

  getImageBlobFromImageMetadata(imageMetadata: ImageMetadata): Observable<{ blob: Blob, fileName: string }> {
    return this.httpClient.get(imageMetadata.url, {responseType: 'blob'}).pipe(
      map(blob => ({blob: blob, fileName: imageMetadata.name}))
    );
  }
}
