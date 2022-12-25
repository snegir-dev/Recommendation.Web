import {Component, OnInit, Renderer2} from '@angular/core';
import {LanguageService} from "../common/services/translate/language.service";
import {AuthService} from "../common/services/auths/auth.service";
import {ReviewQueryService} from "../common/services/routers/review.query.service";
import {ThemeService} from "../common/services/fetches/theme.service";

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['../styles.sass'],
  providers: [AuthService, ReviewQueryService]
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
