import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { Toast } from '../models/toast';

@Injectable({
  providedIn: 'root',
})

export class ToastService {
  private toastSubject = new BehaviorSubject<Toast | null>(null);

  toast$ = this.toastSubject.asObservable();

  showSuccess(message: string): void {
    this.toastSubject.next({
      message,

      type: 'success',
    });

    this.clearToast();
  }

  showError(message: string): void {
    this.toastSubject.next({
      message,

      type: 'error',
    });

    this.clearToast();
  }

  private clearToast() {
    setTimeout(() => {
      this.toastSubject.next(null);
    }, 3000);
  }
}
