import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { ProductService } from '../../../core/services/product';
import { AuthService } from '../../../core/services/auth';
import { Product } from '../../../core/models/product';
import { CurrentUser } from '../../../core/models/current-user';
import { ToastService } from '../../../core/services/toast';

@Component({
  selector: 'app-product-list',
  imports: [ReactiveFormsModule],
  templateUrl: './product-list.html',
  styleUrl: './product-list.css',
})
export class ProductList implements OnInit {
  products: Product[] = [];

  currentUser?: CurrentUser;

  isLoading = false;

  productForm!: FormGroup;

  isEditMode = false;

  selectedProductId = 0;

  constructor(
    private productService: ProductService,

    private authService: AuthService,

    private fb: FormBuilder,

    private toastService: ToastService,
  ) {}

  ngOnInit(): void {
    this.productForm = this.fb.group({
      name: ['', Validators.required],

      description: ['', Validators.required],

      price: ['', Validators.required],
    });

    this.loadProducts();

    this.loadCurrentUser();
  }

  loadProducts(): void {
    this.isLoading = true;

    this.productService.getAllProducts().subscribe({
      next: (response) => {
        this.products = response;

        this.isLoading = false;
      },

      error: (error) => {
        console.log(error);

        this.isLoading = false;
      },
    });
  }

  loadCurrentUser(): void {
    this.authService.getCurrentUser().subscribe({
      next: (response) => {
        this.currentUser = response;
      },

      error: (error) => {
        console.log(error);
      },
    });
  }

  isAdmin(): boolean {
    return this.currentUser?.role === 'Admin';
  }

  createProduct(): void {
    if (this.productForm.invalid) {
      this.productForm.markAllAsTouched();

      return;
    }

    this.productService.createProduct(this.productForm.value).subscribe({
      next: () => {
        this.productForm.reset();

        this.loadProducts();

        this.toastService.showSuccess('Product Created Successfully');
      },
      error: () => {
        this.toastService.showError('Product Creation Failed');
      },
    });
  }

  deleteProduct(id: number): void {
    const confirmed = confirm('Are you sure you want to delete this product?');

    if (!confirmed) {
      return;
    }

    this.productService.deleteProduct(id).subscribe({
      next: () => {
        this.loadProducts();

        this.toastService.showSuccess('Product Deleted Successfully');
      },

      error: () => {
        this.toastService.showError('Delete Failed');
      },
    });
  }

  editProduct(product: Product): void {
    this.isEditMode = true;

    this.selectedProductId = product.id;

    this.productForm.patchValue({
      name: product.name,

      description: product.description,

      price: product.price,
    });
  }

  updateProduct(): void {
    if (this.productForm.invalid) {
      return;
    }

    this.productService
      .updateProduct(
        this.selectedProductId,

        this.productForm.value,
      )
      .subscribe({
        next: () => {
          this.isEditMode = false;

          this.selectedProductId = 0;

          this.productForm.reset();

          this.loadProducts();
        },

        error: (error) => {
          console.log(error);
        },
      });
  }
}
