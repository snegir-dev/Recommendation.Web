import {Component, OnInit} from '@angular/core';
import {ReviewService} from "../../common/services/review.service";
import {ReviewDisplayDto} from "../../common/models/Review/ReviewDisplayDto";
import {SortingService} from "../../common/services/actions/sorting.service";
import {FiltrationService} from "../../common/services/actions/filtration.service";
import {RouterService} from "../../common/services/routers/router.service";
import {UserService} from "../../common/services/fetches/user.service";
import {Router} from "@angular/router";

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.sass'],
  providers: [RouterService]
})
export class ProfileComponent implements OnInit {
  constructor(private reviewService: ReviewService,
              private userService: UserService,
              private router: Router,
              private routerService: RouterService,
              public filtrationService: FiltrationService,
              public sortingService: SortingService) {
  }

  waiter!: Promise<boolean>;
  reviews!: ReviewDisplayDto[];

  ngOnInit(): void {
    this.fetchReviews();
  }

  fetchReviews() {
    let userId: string = this.routerService.getValueFromQueryParams('userId');
    this.reviewService.getByUserId(userId).subscribe({
      next: reviews => {
        this.reviews = reviews;
        this.sortingService.setReviews(reviews);
        this.filtrationService.setParams(this.reviews, 'filtration-container');
        this.waiter = Promise.resolve(true);
      }
    });
  }

  onLogout() {
    this.userService.logout().subscribe({
      next: _ => this.router.navigate(['/login'])
    });
  }
}
