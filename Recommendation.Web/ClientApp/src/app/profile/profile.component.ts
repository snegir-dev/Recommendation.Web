import {Component, OnInit} from '@angular/core';
import {SortingService} from "../../common/services/actions/sorting.service";
import {FiltrationService} from "../../common/services/actions/filtration.service";
import {RouterService} from "../../common/services/routers/router.service";
import {UserService} from "../../common/services/fetches/user.service";
import {Router} from "@angular/router";
import {AuthService} from "../../common/services/auths/auth.service";
import { ReviewService } from 'src/common/services/fetches/review.service';
import {ReviewDisplayDto} from "../../common/models/review/review.display.dto";

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
              private authService: AuthService,
              private routerService: RouterService,
              public filtrationService: FiltrationService,
              public sortingService: SortingService) {
  }

  waiter!: Promise<boolean>;
  reviews!: ReviewDisplayDto[];
  userId!: string | null;

  ngOnInit(): void {
    this.fetchReviews();
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

  onLogout() {
    this.userService.logout().subscribe({
      next: _ => {
        this.authService.isAuthenticate = false;
        this.router.navigate(['/login']);
      }
    });
  }
}
