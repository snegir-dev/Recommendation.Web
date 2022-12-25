import {Component, OnInit} from '@angular/core';
import {SortingService} from "../../common/services/actions/sorting.service";
import {FiltrationService} from "../../common/services/actions/filtration.service";
import {RouterService} from "../../common/services/routers/router.service";
import {UserService} from "../../common/services/fetches/user.service";
import {Router} from "@angular/router";
import {AuthService} from "../../common/services/auths/auth.service";
import { ReviewService } from 'src/common/services/fetches/review.service';
import {ReviewCardDto} from "../../common/models/review/reviewCardDto";
import {UserInfo} from "../../common/models/user/user.info";

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
              public authService: AuthService,
              private routerService: RouterService,
              public filtrationService: FiltrationService,
              public sortingService: SortingService) {
  }

  waiter!: Promise<boolean>;
  reviews!: ReviewCardDto[];
  userInfo!: UserInfo;
  userId!: string | null;

  ngOnInit(): void {
    this.fetchReviews();
    this.fetchUser();
  }

  fetchReviews() {
    this.userId = this.routerService.getValueFromParams<string>('userId') || null;
    this.reviewService.getByUserIdOrDefault(this.userId).subscribe({
      next: reviews => {
        this.reviews = reviews;
        this.sortingService.setReviews(reviews);
        this.filtrationService.setParams(this.reviews, 'filtration-container');
        this.waiter = Promise.resolve(true);
      }
    });
  }

  fetchUser() {
    this.userService.getUserInfo().subscribe({
      next: userInfo => this.userInfo = userInfo
    });
  }

  onLogout() {
    this.userService.logout().subscribe({
      next: _ => {
        this.authService.isAuthenticate = false;
        this.router.navigate(['/login']);
      }
    });
  }
}
