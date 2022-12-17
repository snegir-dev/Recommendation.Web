import {Component, OnInit} from '@angular/core';
import {UserService} from "../../common/services/fetches/user.service";
import {UserModel} from "../../common/models/user/user.model";

@Component({
  selector: 'app-admin',
  templateUrl: './admin.component.html',
  styleUrls: ['./admin.component.sass']
})
export class AdminComponent implements OnInit {
  constructor(private userService: UserService) {
  }

  users: UserModel[] = [];

  ngOnInit(): void {
    this.userService.getUsers().subscribe({
      next: users => this.users = users
    })
  }
}
