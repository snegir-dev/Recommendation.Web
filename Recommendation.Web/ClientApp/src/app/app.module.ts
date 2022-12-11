import {BrowserModule} from '@angular/platform-browser';
import {NgModule, SecurityContext} from '@angular/core';
import {FormsModule, ReactiveFormsModule} from '@angular/forms';
import {HttpClient, HttpClientModule} from '@angular/common/http';
import {RouterModule} from '@angular/router';

import {AppComponent} from './app.component';
import {HomeComponent} from './home/home.component';
import {RegistrationComponent} from "./registration/registration.component";
import {HeaderComponent} from "./header/header.component";
import {LoginComponent} from "./login/login.component";
import {ThemeToggleComponent} from "./theme-toggle/theme-toggle.component";
import {LoginCallbackComponent} from "./login-callback/login-callback.component";
import {ExternalLoginComponent} from "./external-login/external-login.component";
import {CreateReviewComponent} from "./create-review/create-review.component";
import {NgbRatingModule} from "@ng-bootstrap/ng-bootstrap";
import {NgxDropzoneModule} from 'ngx-dropzone';
import {NgxTagsInputBoxModule} from "ngx-tags-input-box";
import {TagInputModule} from "ngx-chips";
import {BrowserAnimationsModule} from "@angular/platform-browser/animations";
import {MarkdownModule, MarkedOptions} from 'ngx-markdown';
import {MarkdownEditorModule} from "./markdown-editor/markdown-editor.module";
import {ReviewFromModule} from "./review-from/review-from.module";
import {MissingTranslationHandler, TranslateLoader, TranslateModule} from '@ngx-translate/core';
import {TranslateHttpLoader} from "@ngx-translate/http-loader";
import {LanguageToggleComponent} from "./language-toggle/language-toggle.component";
import {ReviewCardComponent} from "./review-card/review-card.component";
import {NgxPaginationModule} from "ngx-pagination";
import {ReviewViewComponent} from "./review-view/review-view.component";
import {ReviewCommentsComponent} from "./review-comments/review-comments.component";

@NgModule({
  bootstrap: [AppComponent],
  declarations: [
    AppComponent,
    HomeComponent,
    RegistrationComponent,
    LoginComponent,
    HeaderComponent,
    ThemeToggleComponent,
    LanguageToggleComponent,
    LoginCallbackComponent,
    ExternalLoginComponent,
    CreateReviewComponent,
    ReviewCardComponent,
    ReviewViewComponent,
    ReviewCommentsComponent,
    ReviewCommentsComponent
  ],
  imports: [
    BrowserModule.withServerTransition({appId: 'ng-cli-universal'}),
    HttpClientModule,
    FormsModule,
    RouterModule.forRoot([
      {path: '', component: HomeComponent, pathMatch: 'full'},
      {path: 'login-callback', component: LoginCallbackComponent},
      {path: 'registration', component: RegistrationComponent},
      {path: 'login', component: LoginComponent},
      {path: 'create-review', component: CreateReviewComponent},
      {path: 'view-review/:id', component: ReviewViewComponent}
    ]),
    ReactiveFormsModule,
    NgbRatingModule,
    NgxDropzoneModule,
    NgxTagsInputBoxModule,
    TagInputModule,
    BrowserAnimationsModule,
    MarkdownEditorModule,
    ReviewFromModule,
    MarkdownModule.forRoot(({
      markedOptions: {
        provide: MarkedOptions,
        useValue: {
          gfm: true,
          breaks: true,
          pedantic: false,
          smartLists: true,
          smartypants: true
        },
      },
      sanitize: SecurityContext.NONE
    })),
    TranslateModule.forRoot({
      loader: {
        provide: TranslateLoader,
        useFactory: HttpLoaderFactory,
        deps: [HttpClient],
      },
      useDefaultLang: false,
    }),
    NgxPaginationModule
  ],
  providers: []
})
export class AppModule {
}

export function HttpLoaderFactory(http: HttpClient): TranslateLoader {
  return new TranslateHttpLoader(http, './assets/locale/', '.json');
}
