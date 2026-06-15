import { Component } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from '../../../core/services/auth';
import { ToastService } from '../../../core/services/toast';

@Component({
  selector: 'app-register',
  imports: [ReactiveFormsModule],
  templateUrl: './register.html',
  styleUrl: './register.css',
})
export class RegisterComponent {
  registerForm: FormGroup;

  backendError: string = '';

  constructor(
    private fb: FormBuilder,
    private authService: AuthService,
    private router: Router,
    private toastService: ToastService,
  ) {
    this.registerForm = this.fb.group({
      firstName: ['', Validators.required],

      lastName: ['', Validators.required],

      email: ['', [Validators.required, Validators.email]],

      password: ['', [Validators.required, Validators.minLength(6)]],
    });
  }

  onSubmit(): void {
    this.backendError = '';

    if (this.registerForm.invalid) {
      this.registerForm.markAllAsTouched();
      return;
    }

    this.authService.register(this.registerForm.value).subscribe({
      next: () => {
        this.toastService.showSuccess('Registration Successful');

        this.router.navigate(['/login']);
      },

      error: (error) => {
        this.toastService.showError('Registration Failed');

        if (error?.error?.title) {
          this.backendError = error.error.title;
        } else {
          this.backendError = 'Registration failed. Please try again.';
        }
      },
    });
  }
}
