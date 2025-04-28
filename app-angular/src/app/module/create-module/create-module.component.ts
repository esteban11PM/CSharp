import { NgIf } from '@angular/common';
import { Component, inject } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatFormField } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { Router } from '@angular/router';
import { ModelService } from '../../services/model.service';

@Component({
  selector: 'app-from-module',
  imports: [ReactiveFormsModule, MatFormField, MatInputModule, MatButtonModule, NgIf],
  templateUrl: './create-module.component.html',
  styleUrl: './create-module.component.css',
  standalone: true
})
export class CreateModuleComponent {

  private formBuilder = inject(FormBuilder);
  private moduleService = inject(ModelService);
  private router = inject(Router);

  form = this.formBuilder.group({
    name: ['', Validators.required],
    description: [''],
    active: [true, Validators.required],
  });
  submit(): void {
    if (this.form.invalid) return;

    this.moduleService.create(this.form.value).subscribe({
      next: () => this.router.navigate(['/module']),
      error: err => alert("Error al registrar: " + err.message)
    });
  }
  cancelar(): void {
    this.router.navigate(['/module']);
  }

}
