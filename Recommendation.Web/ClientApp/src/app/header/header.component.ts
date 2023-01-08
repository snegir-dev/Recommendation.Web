import {Component} from '@angular/core';
import {AuthService} from "../../common/services/auths/auth.service";
import {Router} from "@angular/router";
import {ReviewQueryService} from "../../common/services/routers/review.query.service";

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.sass']
})
export class HeaderComponent {
  constructor(public authService: AuthService,
              private router: Router,
              private reviewQueryService: ReviewQueryService) {
  }

  onSubmit() {
    this.router.navigate(['/'], {
      queryParams: {
        'searchText': this.reviewQueryService.searchText,
        'filter': this.reviewQueryService.filter,
        'tag': this.reviewQueryService.tag,
        'numberPage': 1,
        'pageSize': this.reviewQueryService.pageSize
      }
    });
  }

  onChange(value: string) {
    this.reviewQueryService.searchText = value;

    if (value === '' || value === null || value == undefined) {
      this.onSubmit();
    }
  }
}
