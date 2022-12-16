import {Component, OnInit} from '@angular/core';
import {AuthService} from "../../common/services/auths/auth.service";
import {ClaimNames} from "../../common/models/auth/claim.names";

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.sass']
})
export class HeaderComponent implements OnInit {
  constructor(public authService: AuthService) {
  }

  userId!: string;

  ngOnInit(): void {
    this.authService.getValueClaim(ClaimNames.nameIdentifier).subscribe({
      next: userId => {
        this.userId = userId
      }
    })
  }
}
