import { Component, inject, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { ModelService } from '../../services/model.service';
import { CommonModule } from '@angular/common';
import { MatSelectModule } from '@angular/material/select';

@Component({
  selector: 'app-update-module',
  standalone: true,
  imports: [
    ReactiveFormsModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    MatSelectModule,
    CommonModule
  ],
  templateUrl: './update-module.component.html',
  styleUrl: './update-module.component.css'
})
export class UpdateModuleComponent implements OnInit {
  private fb = inject(FormBuilder);
  private moduleService = inject(ModelService);
  private route = inject(ActivatedRoute);
  private router = inject(Router);

  form!: FormGroup;
  moduleId!: number;

  ngOnInit(): void {
    this.moduleId = Number(this.route.snapshot.paramMap.get('id'));
    this.form = this.fb.group({
      id: [this.moduleId],
      name: ['', Validators.required],
      description: [''],
      active: [true, Validators.required],
    });

    this.moduleService.getById(this.moduleId).subscribe(module => {
      this.form.patchValue({
        name: module.name,
        description: module.description,
        active: module.active,
      });
    });
  }

  submit(): void {
    if (this.form.invalid) return;

    const payload = {
      ...this.form.value,
      active: this.form.value['active'] === 'true',
    };

    this.moduleService.update(payload).subscribe({
      next: () => this.router.navigate(['/module']),
      error: err => alert('Error al actualizar: ' + err.message),
    });
  }

  cancelar(): void {
    this.router.navigate(['/module']);
  }
}
