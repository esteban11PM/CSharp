import { Component, OnInit, inject } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AccesoService } from '../../services/acceso.service';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { NgIf } from '@angular/common';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { LoginRequest } from '../../Interfaces/Login';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [ReactiveFormsModule, MatFormFieldModule, MatInputModule, MatButtonModule, MatSnackBarModule, NgIf],
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})
export class LoginComponent implements OnInit {

  private formBuilder = inject(FormBuilder);
  private authService = inject(AccesoService);
  private router = inject(Router);
  private snackBar = inject(MatSnackBar);

  errorMessage: string | null = null;

  form = this.formBuilder.group({
    username: ['', Validators.required],
    password: ['', Validators.required]
  });

  ngOnInit(): void {}

  mostrarBienvenida = false;
  submit(): void {
    if (this.form.invalid) return;

    const loginData: LoginRequest = {
      username: this.form.value.username || '', // Aseguramos que sea string
      password: this.form.value.password || ''
    };
    this.authService.login(loginData).subscribe({
      next: () => {
        this.mostrarBienvenida = true;

        setTimeout(() => {
          this.router.navigate(['/user']);
        }, 1500);
      },
      error: err => {
        const message = err.status === 401
          ? 'La contraseña o el usuario son incorrectos'
          : err.error?.message || 'Error en l inicio de sesión';

        this.snackBar.open(message, 'Cerrar', {
          duration: 4000,
          horizontalPosition: 'center',
          verticalPosition:  'top',
          panelClass: ['error-snackbar']
        });
      }
    });
  }

  irARegistro(): void {
    this.router.navigate(['/login']);
  }
}
