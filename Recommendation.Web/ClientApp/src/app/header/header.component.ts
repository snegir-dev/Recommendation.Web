import {Component, OnInit} from '@angular/core';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./styles/header.component.sass']
})
export class HeaderComponent implements OnInit {
  changeTheme(themeName: string) {
    let mainBody = document.getElementById('main-body')!;
    mainBody.className = themeName;

    this.saveTheme(themeName);
  }

  saveTheme(themeName: string) {
    localStorage.setItem('theme', themeName);
  }

  ngOnInit(): void {
    let theme: string = localStorage.getItem('theme') || 'light-theme';
    this.changeTheme(theme);
  }
}
