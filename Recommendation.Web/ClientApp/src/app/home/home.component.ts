import {Component, OnInit} from '@angular/core';
import {ReviewService} from "../../common/services/review.service";
import {ReviewDto} from "../../common/models/Review/ReviewDto";
import {ActivatedRoute, Router} from "@angular/router";

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['home.component.sass']
})
export class HomeComponent implements OnInit {
  constructor(private reviewService: ReviewService,
              private route: ActivatedRoute,
              private router: Router) {
  }

  waiter!: Promise<boolean>;
  page = 1;
  pageSize: number = 10;
  totalCountReviews = 0;
  reviewPreviews = new Array<ReviewDto>();
  searchText!: string;

  ngOnInit(): void {
    this.changeRoute();
    this.route.queryParams.subscribe((queryParam: any) => {
        this.searchText = queryParam['searchText'];
        this.page = queryParam['numberPage'];
        this.pageSize = queryParam['pageSize'];
      }
    );
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
