import {Component, OnInit} from '@angular/core';
import {ActivatedRoute, Router} from "@angular/router";
import {RouterService} from "../../common/services/routers/router.service";
import {TagService} from "../../common/services/fetches/tag.service";
import {ReviewQueryService} from "../../common/services/routers/review.query.service";
import { ReviewService } from 'src/common/services/fetches/review.service';
import {ReviewDisplayDto} from "../../common/models/review/review.display.dto";
import {FiltrationType} from "../../common/constants/filtration.type";

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['home.component.sass'],
  providers: [RouterService]
})
export class HomeComponent implements OnInit {
  constructor(public reviewQueryService: ReviewQueryService,
              private reviewService: ReviewService,
              private tagService: TagService,
              private route: ActivatedRoute,
              private router: Router) {
  }

  filtrationType: FiltrationType = new FiltrationType();
  waiter!: Promise<boolean>;
  totalCountReviews = 0;
  reviewPreviews = new Array<ReviewDisplayDto>();
  tags: string[] = [];

  ngOnInit(): void {
    this.route.queryParams.subscribe((queryParam: any) => {
        if (Object.keys(queryParam).length == 0)
          this.changeRoute();
        this.reviewQueryService.page = queryParam['numberPage'];
        this.reviewQueryService.filter = queryParam['filter'];
        this.reviewQueryService.tag = queryParam['tag'];
        this.fetchReviews(queryParam);
      }
    );
    this.fetchTags();
  }

  onChangeFilter(filter: string) {
    this.reviewQueryService.filter = filter;
    this.reviewQueryService.page = 1;
    this.changeRoute();
  }

  onChangeTag(tag: string | null) {
    this.reviewQueryService.tag = tag;
    this.reviewQueryService.page = 1;
    this.changeRoute();
  }

  handlePageChange(event: any) {
    this.reviewQueryService.page = event;
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
        'searchText': this.reviewQueryService.searchText,
        'filter': this.reviewQueryService.filter,
        'tag': this.reviewQueryService.tag,
        'numberPage': this.reviewQueryService.page,
        'pageSize': this.reviewQueryService.pageSize
      }
    });
  }
}
