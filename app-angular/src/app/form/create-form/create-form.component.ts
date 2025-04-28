import { NgIf } from '@angular/common';
import { Component, inject } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatFormField } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { Router } from '@angular/router';
import { FormService } from '../../services/form.service';

@Component({
  selector: 'app-create-form',
  imports: [ReactiveFormsModule, MatFormField, MatInputModule, MatButtonModule, NgIf],
  templateUrl: './create-form.component.html',
  styleUrl: './create-form.component.css',
  standalone: true
})
export class CreateFormComponent {

  private formBuilder = inject(FormBuilder);
  private formService = inject(FormService);
  private router = inject(Router);

  form = this.formBuilder.group({
    name: ['', Validators.required],
    description: [''],
    active: [true, Validators.required],
  });
  submit(): void {
    if (this.form.invalid) return;

    this.formService.create(this.form.value).subscribe({
      next: () => this.router.navigate(['/form']),
      error: err => alert("Error al registrar: " + err.message)
    });
  }
  cancelar(): void {
    this.router.navigate(['/form']);
  }

}
