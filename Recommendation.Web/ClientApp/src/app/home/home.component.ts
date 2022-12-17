import {Component, OnInit} from '@angular/core';
import {ReviewService} from "../../common/services/review.service";
import {ReviewDisplayDto} from "../../common/models/Review/ReviewDisplayDto";
import {ActivatedRoute, Router} from "@angular/router";
import {UserService} from "../../common/services/fetches/user.service";
import {AuthService} from "../../common/services/auths/auth.service";
import {RouterService} from "../../common/services/routers/router.service";
import {TagService} from "../../common/services/fetches/tag.service";

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['home.component.sass'],
  providers: [RouterService]
})
export class HomeComponent implements OnInit {
  constructor(private reviewService: ReviewService,
              private tagService: TagService,
              private route: ActivatedRoute,
              private router: Router) {
  }

  waiter!: Promise<boolean>;
  page: number = 1;
  pageSize: number = 10;
  totalCountReviews = 0;
  reviewPreviews = new Array<ReviewDisplayDto>();
  searchText!: string;
  filter: string = 'date';
  tag!: string | null;
  tags: string[] = [];

  ngOnInit(): void {
    this.route.queryParams.subscribe((queryParam: any) => {
        if (Object.keys(queryParam).length == 0)
          this.changeRoute();
        this.page = queryParam['numberPage'];
        this.filter = queryParam['filter'];
        this.tag = queryParam['tag'];
        this.fetchReviews(queryParam);
      }
    );
    this.fetchTags();
  }

  onChangeFilter(filter: string) {
    this.filter = filter;
    this.page = 1;
    this.changeRoute();
  }

  onChangeTag(tag: string | null) {
    this.tag = tag;
    this.page = 1;
    this.changeRoute();
  }

  handlePageChange(event: any) {
    this.page = event;
    this.changeRoute();
  }

  fetchReviews(params: any): void {
    this.reviewService.getByParams(params).subscribe({
      next: value => {
        this.reviewPreviews = value.reviewDtos;
        this.totalCountReviews = value.totalCountReviews;
        window.scroll({top: 0});
        this.waiter = Promise.resolve(true);
      },
      error: err => console.log(err)
    });
  }

  fetchTags() {
    this.tagService.getTags().subscribe({
      next: tags => this.tags = tags
    });
  }

  changeRoute() {
    this.router.navigate(['/'], {
      queryParams: {
        'searchText': this.searchText,
        'filter': this.filter,
        'tag': this.tag,
        'numberPage': this.page,
        'pageSize': this.pageSize
      }
    });
  }
}
