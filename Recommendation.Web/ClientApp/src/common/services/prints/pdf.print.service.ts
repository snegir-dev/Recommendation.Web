import {ElementRef, Injectable} from "@angular/core";
import * as pdfMake from "pdfmake/build/pdfmake";
import * as pdfFonts from "pdfmake/build/vfs_fonts";
import {ImageService} from "../fetches/image.service";

const htmlToPdfmake = require("html-to-pdfmake");
(pdfMake as any).vfs = pdfFonts.pdfMake.vfs;

@Injectable({
  providedIn: 'root'
})
export class PdfPrintService {
  constructor(private imageService: ImageService) {
  }

  private readonly a4Width: number = 797;
  pdfSection!: ElementRef;

  async createPdf(excludeClass: string) {
    const convertedHtml = await this.convertHtml(excludeClass);
    const html = htmlToPdfmake(convertedHtml);
    const documentDefinition = {content: html};
    pdfMake.createPdf(documentDefinition).download();
  }

  private async convertHtml(excludeClass: string): Promise<any> {
    let copyPdfSection = this.removeExcludedElements(excludeClass);
    let elements = copyPdfSection.querySelectorAll('img');

    for (let element of elements) {
      if (element.width > this.a4Width) {
        element.width = this.a4Width - 100;
      }

      element.style.marginTop = '10px';
      element.src = await this.imageService.getBase64Image(element.src);
    }

    return copyPdfSection.innerHTML;
  }

  private removeExcludedElements(excludeClass: string): any {
    let copyPdfSection = this.pdfSection.nativeElement.cloneNode(true);
    let excludedElements = copyPdfSection
      .querySelectorAll(`.${excludeClass}`);

    for (const element of excludedElements) {
      element.remove();
    }

    return copyPdfSection;
  }
}
