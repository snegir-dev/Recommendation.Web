import {Component, OnInit} from '@angular/core';
import {ThemeService} from "../../common/services/fetches/theme.service";

@Component({
  selector: 'app-theme-toggle',
  templateUrl: './theme-toggle.component.html',
  styleUrls: ['./theme-toggle.component.sass']
})
export class ThemeToggleComponent implements OnInit {
  constructor(private themeService: ThemeService) {
  }

  ngOnInit(): void {
    let theme: string = localStorage.getItem('theme') || 'light';
    this.toggleTheme(theme);
  }

  toggleTheme(theme: string) {
    localStorage.setItem('theme', theme);

    if (theme === 'light') {
      theme = 'bootstrap';
    } else {
      theme = 'bootstrap-dark';
    }

    this.themeService.setTheme(theme);
  }
}
