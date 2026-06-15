import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { LoginRequest } from '../models/login-request';
import { LoginResponse } from '../models/login-response';
import { RegisterRequest } from '../models/register-request';
import { CurrentUser } from '../models/current-user';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  constructor(private http: HttpClient) {}

  login(request: LoginRequest): Observable<LoginResponse> {
    return this.http.post<LoginResponse>(`${environment.apiUrl}/auth/login`, request);
  }

  saveToken(token: string, refreshToken: string): void {
    localStorage.setItem('token', token);
    localStorage.setItem('refreshToken', refreshToken);
  }

  getToken(): string | null {
    return localStorage.getItem('token');
  }

  isLoggedIn(): boolean {
    return !!localStorage.getItem('token');
  }

  register(request: RegisterRequest): Observable<any> {
    return this.http.post(`${environment.apiUrl}/auth/register`, request);
  }

  logout(): void {
    localStorage.removeItem('token');

    localStorage.removeItem('refreshToken');
  }

  getCurrentUser() {
    return this.http.get<CurrentUser>(`${environment.apiUrl}/auth/me`);
  }
}
