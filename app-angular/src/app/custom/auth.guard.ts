// auth.guard.ts
import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';

export const authGuard: CanActivateFn = () => {
  const router = inject(Router);
  const token = localStorage.getItem('auth_token');

  if (token) {
    return true;
  }

  // Si no hay token, redirige al login
  router.navigate(['/login']);
  return false;
};
