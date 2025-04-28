import { NgFor, NgIf } from '@angular/common';
import { Component, inject, OnInit } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { Router } from '@angular/router';
import { FormService } from '../../services/form.service';
import { PermissionService } from '../../services/permission.service';
import { RolService } from '../../services/rol.service';
import { RolFormPermissionService } from '../../services/rol-form-permission.service';

@Component({
  selector: 'app-create-rol-form-permission',
  standalone: true,
  imports: [ReactiveFormsModule, MatFormFieldModule, MatInputModule, MatSelectModule, MatButtonModule, NgFor, NgIf],
  templateUrl: './create-rol-form-permission.component.html',
  styleUrl: './create-rol-form-permission.component.css'
})
export class CreateRolFormPermissionComponent implements OnInit {

  private formBuilder = inject(FormBuilder);
  private rolFormPermissionService = inject(RolFormPermissionService);
  private rolService = inject(RolService);
  private formService = inject(FormService);
  private permissionService = inject(PermissionService);
  private router = inject(Router);

  roles: any[] = [];
  forms: any[] = [];
  permissions: any[] = [];
  rolFormPermissions: any[] = [];
  noRolesAvailable = false;
  noFormsAvailable = false;
  noPermissionsAvailable = false;

  form = this.formBuilder.group({
    rolId: [null, Validators.required],
    permissionId: [null, Validators.required],
    formId: [null, Validators.required],
    active: [true, Validators.required],
  });

  ngOnInit(): void {
    this.rolService.getAll().subscribe((data) => {
      this.roles = data;
      if (this.roles.length === 0) {
        this.noRolesAvailable = true;
        this.form.disable();
      }
    });

    this.formService.getAll().subscribe((data) => {
      this.forms = data;
      if (this.forms.length === 0) {
        this.noFormsAvailable = true;
        this.form.disable();
      }
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

    console.log('Datos a enviar:', this.form.value); 

    const alreadyExists = this.rolFormPermissions.some(rfp => rfp.rolId === rolId && rfp.permissionId === permissionId && rfp.formId === formId);

    if (alreadyExists) {
      alert('El rol y el permiso ya tienen relación, elige otro');
      return;
    }

    this.rolFormPermissionService.create(this.form.value).subscribe({
      next: () => this.router.navigate(['/rol-form-permission']),
      
      error: err => alert("Error al registrar: " + err.message)
    });
  }
  cancelar(): void {
    this.router.navigate(['/rol-form-permission']);
  }
}
