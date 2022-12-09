import {TranslateService} from "@ngx-translate/core";
import {Injectable, OnInit} from "@angular/core";
import {environment} from "../../../environments/environment";

@Injectable({
  providedIn: 'root'
})
export class LanguageService implements OnInit {
  constructor(private translateService: TranslateService) {
  }

  ngOnInit(): void {
    let lang: string | null = this.getLanguage();
    if (lang === null)
      this.translateService.use(environment.defaultLocale);
    else
      this.changeLanguage(lang);
  }

  changeLanguage(lang: string): void {
    this.translateService.use(lang);
    this.saveLanguage(lang);
  }

  saveLanguage(lang: string): void {
    localStorage.setItem('language', lang);
  }

  getLanguage(): string | null {
    return localStorage.getItem('language');
  }
}
