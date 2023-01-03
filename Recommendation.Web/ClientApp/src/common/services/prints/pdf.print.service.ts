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

  async createPdf(...excludeClasses: string[]) {
    const convertedHtml = await this.convertHtml(excludeClasses);
    const html = htmlToPdfmake(convertedHtml);
    const documentDefinition = {content: html};
    pdfMake.createPdf(documentDefinition).download();
  }

  private async convertHtml(excludeClasses: string[]): Promise<any> {
    let copyPdfSection = this.removeExcludedElements(excludeClasses);
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

  private removeExcludedElements(excludeClasses: string[]): any {
    let copyPdfSection = this.pdfSection.nativeElement.cloneNode(true);
    let excludedElementsLine = excludeClasses.join(' .');
    let excludedElements = copyPdfSection
      .querySelectorAll(excludedElementsLine.slice(0, 0) + '.' + excludedElementsLine.slice(0));

    for (const element of excludedElements) {
      element.remove();
    }

    return copyPdfSection;
  }
}
