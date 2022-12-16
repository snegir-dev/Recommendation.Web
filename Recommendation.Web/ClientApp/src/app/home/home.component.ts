import {Component, OnInit} from '@angular/core';
import {ReviewService} from "../../common/services/review.service";
import {ReviewDisplayDto} from "../../common/models/Review/ReviewDisplayDto";
import {ActivatedRoute, Router} from "@angular/router";
import {UserService} from "../../common/services/fetches/user.service";
import {AuthService} from "../../common/services/auths/auth.service";
import {RouterService} from "../../common/services/routers/router.service";

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['home.component.sass'],
  providers: [RouterService]
})
export class HomeComponent implements OnInit {
  constructor(private reviewService: ReviewService,
              private route: ActivatedRoute,
              private router: Router,
              private routerService: RouterService) {
  }

  waiter!: Promise<boolean>;
  page: number = 1;
  pageSize: number = 1;
  totalCountReviews = 0;
  reviewPreviews = new Array<ReviewDisplayDto>();
  searchText!: string;

  ngOnInit(): void {
    this.searchText = this.routerService.getValueFromQueryParams<string>('searchText');
    this.page = this.routerService.getValueFromQueryParams<number>('numberPage');
    this.pageSize = this.routerService.getValueFromQueryParams<number>('pageSize');
    this.changeRoute();
    this.route.queryParams.subscribe(_ => this.fetchReviews());
  }

  async handlePageChange(event: any) {
    this.page = event;
    await this.changeRoute();
    this.fetchReviews();
  }

  fetchReviews(): void {
    const params = this.getRequestParams(this.searchText, this.page, this.pageSize);

    this.reviewService.getByParams(params).subscribe({
      next: value => {
        this.reviewPreviews = value.reviewDtos;
        this.totalCountReviews = value.totalCountReviews;
        this.waiter = Promise.resolve(true);
        window.scroll({top: 0});
      },
      error: err => console.log(err)
    });
  }

  getRequestParams(searchText: string, page: number, pageSize: number): any {
    let params: any = {};
    params[`searchText`] = searchText;
    params[`numberPage`] = page;
    params[`pageSize`] = pageSize;

    return params;
  }

  changeRoute() {
    this.router.navigate(['/'], {
      queryParams: {
        'searchText': this.searchText,
        'numberPage': this.page,
        'pageSize': this.pageSize
      }
    });
  }
}
