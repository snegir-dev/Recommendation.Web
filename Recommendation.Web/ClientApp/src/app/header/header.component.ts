import {Component, OnInit} from '@angular/core';
import {AuthService} from "../../common/services/auths/auth.service";
import {ClaimNames} from "../../common/models/auth/claim.names";

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.sass']
})
export class HeaderComponent {
  constructor(public authService: AuthService) {
  }
}
