import { Component, OnInit } from '@angular/core';
import { Router, RouterLink } from '@angular/router';
import { AuthService } from '../../../core/services/auth';
import { CurrentUser } from '../../../core/models/current-user';

@Component({
  selector: 'app-navbar',
  imports: [RouterLink],
  templateUrl: './navbar.html',
  styleUrl: './navbar.css',
})
export class NavbarComponent implements OnInit {
  currentUser?: CurrentUser;

  constructor(
    private authService: AuthService,

    private router: Router,
  ) {}

  ngOnInit(): void {
    if (this.isLoggedIn()) {
      this.authService.getCurrentUser().subscribe({
        next: (response) => {
          this.currentUser = response;
        },

        error: (error) => {
          console.log(error);
        },
      });
    }
  }

  isLoggedIn(): boolean {
    return this.authService.isLoggedIn();
  }

  isAdmin(): boolean {
    return this.currentUser?.role === 'Admin';
  }

  logout(): void {
    this.authService.logout();

    this.router.navigate(['/login']);
  }
}
