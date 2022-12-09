import {Component, OnInit} from '@angular/core';
import {ThemeService} from "../../common/services/theme.service";
import {TranslateService} from "@ngx-translate/core";
import {LanguageService} from "../../common/services/translate/language.service";

@Component({
  selector: 'app-language-toggle',
  templateUrl: './language-toggle.component.html',
  styleUrls: ['./language-toggle.component.sass']
})
export class LanguageToggleComponent {
  constructor(private languageService: LanguageService) {
  }

  toggleLanguage(lang: string) {
    this.languageService.changeLanguage(lang);
  }
}
