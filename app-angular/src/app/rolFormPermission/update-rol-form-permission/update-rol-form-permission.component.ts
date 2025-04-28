import { NgForOf } from '@angular/common';
import { Component, inject, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { Router, ActivatedRoute } from '@angular/router';
import { PermissionService } from '../../services/permission.service';
import { RolFormPermissionService } from '../../services/rol-form-permission.service';

@Component({
  selector: 'app-update-rol-form-permission',
  standalone: true,
  imports: [ReactiveFormsModule, MatFormFieldModule, MatInputModule, MatSelectModule, MatButtonModule, NgForOf],
  templateUrl: './update-rol-form-permission.component.html',
  styleUrl: './update-rol-form-permission.component.css'
})
export class UpdateRolFormPermissionComponent implements OnInit {
  private fb = inject(FormBuilder);
  private rolFormPermissionService = inject(RolFormPermissionService);
  private permissionService = inject(PermissionService);
  private router = inject(Router);
  private route = inject(ActivatedRoute);

  permissions: any[] = [];  // permisos disponibles
  rolFormPermissions: any[] = [];
  noPermissionsAvailable = false; // bandera para mostrar mensaje si no hay permisos

  form!: FormGroup;
  rolFormPermissionId!: number;

  ngOnInit(): void {
    this.rolFormPermissionId = Number(this.route.snapshot.paramMap.get('id'));
    this.form = this.fb.group({
      id: [this.rolFormPermissionId],
      rolId: [0], // importante para el update
      permissionId: [0], // importante para el update
      formId: [0], // importante para el update
      active: [true, Validators.required],
    });

    this.rolFormPermissionService.getById(this.rolFormPermissionId).subscribe(rolFormPermission => {
      this.form.patchValue({
        rolId: rolFormPermission.rolId,
        permissionId: rolFormPermission.permissionId,
        formId: rolFormPermission.formId,
        active: rolFormPermission.active,
      });
    });

    this.permissionService.getAll().subscribe((data) => {
      this.permissions = data;
      if (this.permissions.length === 0) {
        this.noPermissionsAvailable = true;
        this.form.disable();
      }
    });

    this.rolFormPermissionService.getAll().subscribe((data) => {
      this.rolFormPermissions = data;
    });
  }

  submit(): void {
    if (this.form.invalid) return;

    const rolId = this.form.value.rolId;
    const permissionId = this.form.value.permissionId;
    const formId = this.form.value.formId;

    const alreadyExists = this.rolFormPermissions.some(rfp => rfp.rolId === rolId && rfp.permissionId === permissionId && rfp.formId === formId);

    if (alreadyExists) {
      alert('El rol y el permiso ya tienen relación, elige otro');
      return;
    }
    this.rolFormPermissionService.update(this.form.value).subscribe({
      next: () => this.router.navigate(['/rol-form-permission']),
      error: err => alert('Error al actualizar: ' + err.message),
    });
  }

  cancelar(): void {
    this.router.navigate(['/rol-form-permission']);
  }

}
