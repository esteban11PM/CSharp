import { inject, Injectable } from '@angular/core';
import { environment } from '../../environments/environment.development';
import { HttpClient } from '@angular/common/http';
import { LoginRequest, LoginResponse } from '../Interfaces/Login';
import { Observable, tap } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AccesoService {

  private ApiUrl = `${environment.apiURL}api/Auth/login`;
  private http = inject(HttpClient);

  constructor() { }

  /** Intenta autenticarse; almacena token en LocalStorage si es exitoso */
  login(credentials: LoginRequest): Observable<LoginResponse> {
    return this.http.post<LoginResponse>(`${this.ApiUrl}`, credentials)
      .pipe(
        tap(res => {
          localStorage.setItem('auth_token', res.token);
          localStorage.setItem('username', res.username);
          localStorage.setItem('userId', res.userId.toString());
          localStorage.setItem('role', res.rol);
        })
      );
    }
  /** El usuario está autenticado si existe un token no vacío */
  isLoggedIn(): boolean {
    return !!localStorage.getItem('auth_token');
  }

  /** Elimina el token de autenticación */
  logout(): void {
    localStorage.removeItem('auth_token');
    localStorage.removeItem('username');
    localStorage.removeItem('userId');
    localStorage.removeItem('role');
  }

  /** Devuelve el token para usar en un interceptor o en peticiones manuales */
  getToken(): string | null {
    return localStorage.getItem('auth_token');
  }

}
