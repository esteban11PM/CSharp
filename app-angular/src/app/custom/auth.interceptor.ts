// auth.interceptor.ts
import { HttpInterceptorFn, HttpRequest, HttpHandler } from '@angular/common/http';
import { inject } from '@angular/core';
import { AccesoService } from '../services/acceso.service';

export const authInterceptor: HttpInterceptorFn = (req, next) => {
  const auth = inject(AccesoService);
  const token = auth.getToken();
  if (token) {
    req = req.clone({
      setHeaders: { Authorization: `Bearer ${token}` }
    });
  }
  return next(req);
};
