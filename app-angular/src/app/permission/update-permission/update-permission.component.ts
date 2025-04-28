import { CommonModule } from '@angular/common';
import { Component, inject, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { ActivatedRoute, Router } from '@angular/router';
import { PermissionService } from '../../services/permission.service';

@Component({
  selector: 'app-update-permission',
  standalone: true,
  imports: [
    ReactiveFormsModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    MatSelectModule,
    CommonModule
  ],
  templateUrl: './update-permission.component.html',
  styleUrl: './update-permission.component.css'
})
export class UpdatePermissionComponent implements OnInit {
  private fb = inject(FormBuilder);
  private permissionService = inject(PermissionService);
  private route = inject(ActivatedRoute);
  private router = inject(Router);

  form!: FormGroup;
  permissionId!: number;

  ngOnInit(): void {
    this.permissionId = Number(this.route.snapshot.paramMap.get('id'));
    this.form = this.fb.group({
      id: [this.permissionId],
      name: ['', Validators.required],
      description: [''],
      active: [true, Validators.required],
    });

    this.permissionService.getById(this.permissionId).subscribe(permission => {
      this.form.patchValue({
        name: permission.name,
        description: permission.description,
        active: permission.active,
      });
    });
  }

  submit(): void {
    if (this.form.invalid) return;

    const payload = {
      ...this.form.value,
      active: this.form.value['active'] === 'true',
    };

    this.permissionService.update(payload).subscribe({
      next: () => this.router.navigate(['/permission']),
      error: err => alert('Error al actualizar: ' + err.message),
    });
  }

  cancelar(): void {
    this.router.navigate(['/permission']);
  }
}
