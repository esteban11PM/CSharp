import { NgIf } from '@angular/common';
import { Component, inject } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatFormField } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { Router } from '@angular/router';
import { RolService } from '../../services/rol.service';

@Component({
  selector: 'app-from-rol',
  imports: [ReactiveFormsModule, MatFormField, MatInputModule, MatButtonModule, NgIf],
  templateUrl: './create-rol.component.html',
  styleUrl: './create-rol.component.css',
  standalone: true
})
export class CreateRolComponent {

  private formBuilder = inject(FormBuilder);
  private rolService = inject(RolService);
  private router = inject(Router);

  form = this.formBuilder.group({
    name: ['', Validators.required],
    description: [''],
    active: [true, Validators.required],
  });
  submit(): void {
    if (this.form.invalid) return;

    this.rolService.create(this.form.value).subscribe({
      next: () => this.router.navigate(['/rol']),
      error: err => alert("Error al registrar: " + err.message)
    });
  }
  cancelar(): void {
    this.router.navigate(['/rol']);
  }

}
