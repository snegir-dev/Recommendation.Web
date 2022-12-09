import {Component, OnInit, Renderer2} from '@angular/core';
import {ThemeService} from "../common/services/theme.service";
import {LanguageService} from "../common/services/translate/language.service";

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['../styles.sass']
})

export class AppComponent implements OnInit {

  constructor(private themeService: ThemeService,
              private renderer: Renderer2,
              private languageService: LanguageService) {
  }

  ngOnInit(): void {
    this.themeService.themeChanges().subscribe(theme => {
      if (theme.oldValue) {
        this.renderer.removeClass(document.body, theme.oldValue);
      }
      this.renderer.addClass(document.body, theme.newValue);
    });
    this.languageService.ngOnInit();
  }

}
