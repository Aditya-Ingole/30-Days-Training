import { Component, OnInit } from '@angular/core';
import { ToastService } from '../../../core/services/toast';
import { Toast } from '../../../core/models/toast';

@Component({
  selector: 'app-toast',
  imports: [],
  templateUrl: './toast.html',
  styleUrl: './toast.css',
})

export class ToastComponent implements OnInit {
  toast: Toast | null = null;

  constructor(private toastService: ToastService) {}

  ngOnInit(): void {
    this.toastService.toast$.subscribe({
      next: (toast) => {
        this.toast = toast;
      },
    });
  }
}
