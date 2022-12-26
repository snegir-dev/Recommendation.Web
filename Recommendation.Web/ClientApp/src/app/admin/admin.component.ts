import {Component, OnInit} from '@angular/core';
import {UserService} from "../../common/services/fetches/user.service";
import {UserModel} from "../../common/models/user/user.model";
import {AuthService} from "../../common/services/auths/auth.service";
import {Router} from "@angular/router";

@Component({
  selector: 'app-admin',
  templateUrl: './admin.component.html',
  styleUrls: ['./admin.component.sass']
})
export class AdminComponent implements OnInit {
  constructor(private userService: UserService,
              private authService: AuthService,
              private router: Router) {
  }

  users: UserModel[] = [];

  ngOnInit(): void {
    this.userService.getUsers().subscribe({
      next: users => this.users = users
    })
  }

  blockUser(userId: string) {
    this.userService.blockUser(userId).subscribe({
      next: _ => {
        this.authService.fetchIsSignedIn().subscribe(value => {
          this.authService.isAuthenticate = value;
          if (!value)
            this.router.navigate(['/login']);
        });
      }
    });
  }

  unblockUser(userId: string) {
    this.userService.unblockUser(userId).subscribe()
  }
}
