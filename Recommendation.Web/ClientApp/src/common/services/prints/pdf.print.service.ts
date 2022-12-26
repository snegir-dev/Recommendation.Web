import {Injectable} from "@angular/core";
import html2canvas from "html2canvas";
import JsPDF from 'jspdf';

@Injectable({
  providedIn: 'root'
})
export class PdfPrintService {
  createPdf(selector: string) {
    const element: any = document.getElementById(selector);

    html2canvas(element).then((canvas) => {
      let fileWidth = 208;
      let fileHeight = (canvas.height * fileWidth) / canvas.width;
      const imgData = canvas.toDataURL('image/png');
      const doc = new JsPDF('p', 'mm', 'a4');
      doc.html(element.innerHTML);
      // doc.addImage(imgData, 'PNG', 0, 0, fileWidth, fileHeight);
      doc.save('review.pdf');
    });
  }
}
