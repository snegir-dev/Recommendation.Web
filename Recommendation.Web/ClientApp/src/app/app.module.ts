import {BrowserModule} from '@angular/platform-browser';
import {Injector, NgModule, SecurityContext} from '@angular/core';
import {FormsModule, ReactiveFormsModule} from '@angular/forms';
import {HTTP_INTERCEPTORS, HttpClient, HttpClientModule} from '@angular/common/http';
import {Router, RouterModule} from '@angular/router';

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
import {TranslateLoader, TranslateModule} from '@ngx-translate/core';
import {TranslateHttpLoader} from "@ngx-translate/http-loader";
import {LanguageToggleComponent} from "./language-toggle/language-toggle.component";
import {ReviewCardComponent} from "./review-card/review-card.component";
import {NgxPaginationModule} from "ngx-pagination";
import {ReviewViewComponent} from "./review-view/review-view.component";
import {ReviewCommentsComponent} from "./review-comments/review-comments.component";
import {ProfileComponent} from "./profile/profile.component";
import {ReviewFormModule} from "./review-form/review-form.module";
import {PreloaderComponent} from "./preloader/preloader.component";
import {UpdateReviewComponent} from "./update-review/update-review.component";
import {AuthService} from "../common/services/auths/auth.service";
import {DragScrollModule} from "ngx-drag-scroll";
import {AdminComponent} from "./admin/admin.component";
import {AccessDeniedComponent} from "./access-denied/access-denied.component";
import {RoleGuard} from 'src/common/guards/role.guard';
import {AuthGuard} from 'src/common/guards/auth.guard';
import {ErrorInterceptor} from "../common/interceptors/error-interceptor.service";
import {ToastComponent} from "./toast/toast.component";
import {NotFoundComponent} from "./not-found/not-found.component";

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
    ReviewCommentsComponent,
    ProfileComponent,
    PreloaderComponent,
    UpdateReviewComponent,
    AdminComponent,
    AccessDeniedComponent,
    ToastComponent,
    NotFoundComponent
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
      {path: 'create-review', component: CreateReviewComponent, canActivate: [AuthGuard]},
      {
        path: 'create-review/:userId', component: CreateReviewComponent, canActivate: [AuthGuard, RoleGuard],
        data: {
          roles: ['Admin']
        }
      },
      {path: 'update-review/:reviewId', component: UpdateReviewComponent, canActivate: [AuthGuard]},
      {
        path: 'update-review/:reviewId/:userId', component: UpdateReviewComponent,
        canActivate: [AuthGuard, RoleGuard],
        data: {
          roles: ['Admin']
        }
      },
      {path: 'view-review/:id', component: ReviewViewComponent},
      {path: 'profile', component: ProfileComponent, canActivate: [AuthGuard]},
      {
        path: 'profile/:userId', component: ProfileComponent, canActivate: [AuthGuard, RoleGuard],
        data: {
          roles: ['Admin']
        }
      },
      {
        path: 'admin', component: AdminComponent, canActivate: [AuthGuard, RoleGuard],
        data: {
          roles: ['Admin']
        }
      },
      {path: 'access-denied', component: AccessDeniedComponent},
      {path: 'not-found', component: NotFoundComponent, pathMatch: 'full'},
      {path: '**', component: NotFoundComponent, pathMatch: 'full'}
    ]),
    ReactiveFormsModule,
    NgbRatingModule,
    NgxDropzoneModule,
    NgxTagsInputBoxModule,
    TagInputModule,
    BrowserAnimationsModule,
    MarkdownEditorModule,
    ReviewFormModule,
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
    NgxPaginationModule,
    DragScrollModule
  ],
  providers: [
    {
      provide: HTTP_INTERCEPTORS,
      useClass: ErrorInterceptor,
      multi: true,
    },
    AuthService,
    AuthGuard,
    RoleGuard
  ]
})
export class AppModule {
}

export function HttpLoaderFactory(http: HttpClient): TranslateLoader {
  return new TranslateHttpLoader(http, './assets/locale/', '.json');
}
