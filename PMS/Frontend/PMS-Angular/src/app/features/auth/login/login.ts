import { Component } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { AuthService } from '../../../core/services/auth';
import { Router, RouterLink } from '@angular/router';
import { ToastService } from '../../../core/services/toast';

@Component({
  selector: 'app-login',
  imports: [
    ReactiveFormsModule,
    RouterLink,
    MatCardModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
  ],
  templateUrl: './login.html',
  styleUrl: './login.css',
})
export class LoginComponent {
  loginForm: FormGroup;

  backendError: string = '';

  constructor(
    private fb: FormBuilder,
    private authService: AuthService,
    private router: Router,
    private toastService: ToastService,
  ) {
    this.loginForm = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
      password: ['', Validators.required],
    });
  }

  onSubmit(): void {
    this.backendError = '';

    if (this.loginForm.invalid) {
      this.loginForm.markAllAsTouched();
      return;
    }

    this.authService.login(this.loginForm.value).subscribe({
      next: (response) => {
        this.authService.saveToken(response.token, response.refreshToken);

        this.toastService.showSuccess('Login Successful');

        this.router.navigate(['/dashboard']);
      },

      error: (error) => {
        console.log(error);

        this.backendError =
          error?.error?.message || error?.error?.title || 'Login failed. Please try again.';
      },
    });
  }
}
