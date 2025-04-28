import { NgIf } from '@angular/common';
import { Component, inject } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatFormField } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { Router } from '@angular/router';
import { PermissionService } from '../../services/permission.service';

@Component({
  selector: 'app-create-permission',
  imports: [ReactiveFormsModule, MatFormField, MatInputModule, MatButtonModule, NgIf],
  templateUrl: './create-permission.component.html',
  styleUrl: './create-permission.component.css',
  standalone: true
})
export class CreatePermissionComponent {

  private formBuilder = inject(FormBuilder);
  private permissionService = inject(PermissionService);
  private router = inject(Router);

  form = this.formBuilder.group({
    name: ['', Validators.required],
    description: [''],
    active: [true, Validators.required],
  });
  submit(): void {
    if (this.form.invalid) return;

    this.permissionService.create(this.form.value).subscribe({
      next: () => this.router.navigate(['/permission']),
      error: err => alert("Error al registrar: " + err.message)
    });
  }
  cancelar(): void {
    this.router.navigate(['/permission']);
  }

}
