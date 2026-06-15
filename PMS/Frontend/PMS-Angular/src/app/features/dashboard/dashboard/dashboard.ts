import { Component, OnInit } from '@angular/core';
import { RouterLink } from '@angular/router';
import { DatePipe } from '@angular/common';
import { CurrentUser } from '../../../core/models/current-user';
import { AuthService } from '../../../core/services/auth';
import { ProductService } from '../../../core/services/product';

@Component({
  selector: 'app-dashboard',
  imports: [RouterLink, DatePipe],
  templateUrl: './dashboard.html',
  styleUrl: './dashboard.css',
})
export class Dashboard implements OnInit {
  currentUser?: CurrentUser;

  totalProducts = 0;

  currentDate = new Date();

  constructor(
    private authService: AuthService,

    private productService: ProductService,
  ) {}

  ngOnInit(): void {
    this.loadDashboard();
  }

  loadDashboard(): void {
    this.authService.getCurrentUser().subscribe({
      next: (user) => {
        this.currentUser = user;
      },

      error: (error) => {
        console.log(error);
      },
    });

    this.productService.getAllProducts().subscribe({
      next: (products) => {
        this.totalProducts = products.length;
      },

      error: (error) => {
        console.log(error);
      },
    });
  }
}
