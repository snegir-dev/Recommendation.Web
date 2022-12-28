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

  loadWaiter: boolean = false;
  users: UserModel[] = [];

  ngOnInit(): void {
    this.userService.getUsers().subscribe({
      next: users => {
        this.users = users;
        this.loadWaiter = true;
      }
    })
  }

  blockUser(userId: string) {
    this.userService.blockUser(userId).subscribe({
      next: _ => {
        this.authService.fetchIsSignedIn().subscribe(value => {
          let user = this.users.find(user => user.id === userId);
          if (user)
            user.accessStatus = 'Block'

          this.authService.isAuthenticate = value;
          if (!value)
            this.router.navigate(['/login']);
        });
      }
    });
  }

  unblockUser(userId: string) {
    this.userService.unblockUser(userId).subscribe({
      next: _ => {
        let user = this.users.find(user => user.id === userId);
        if (user)
          user.accessStatus = 'Unblock'
      }
    });
  }

  deleteUser(userId: string) {
    this.userService.deleteUser(userId).subscribe({
      next: _ => {
        this.authService.fetchIsSignedIn().subscribe(value => {
          this.authService.isAuthenticate = false;
          this.users = this.users.filter(user => user.id !== userId);
          if (!value)
            this.router.navigate(['/login']);
        });
      }
    });
  }

  setUserRole(userId: string, roleName: string) {
    console.log(roleName)
    this.userService.setUserRole(userId, roleName).subscribe({
      next: _ => {
        let user = this.users.find(user => user.id === userId);
        if (user)
          user.role = roleName;
      }
    });
  }
}
