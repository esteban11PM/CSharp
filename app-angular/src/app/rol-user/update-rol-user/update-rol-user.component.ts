import { Component, inject, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { ActivatedRoute, Router } from '@angular/router';
import { rolUserService } from '../../services/rol-user.service';
import { NgForOf } from '@angular/common';
import { RolService } from '../../services/rol.service';

@Component({
  selector: 'app-update-rol-user',
  standalone: true,
  imports: [ReactiveFormsModule, MatFormFieldModule, MatInputModule, MatSelectModule, MatButtonModule, NgForOf],
  templateUrl: './update-rol-user.component.html',
  styleUrl: './update-rol-user.component.css'
})
export class UpdateRolUserComponent implements OnInit {
  private fb = inject(FormBuilder);
  private rolUserService = inject(rolUserService);
  private rolService = inject(RolService);
  private route = inject(ActivatedRoute);
  private router = inject(Router);

  roles: any[] = [];  // roles disponibles
  rolUsers: any[] = []; // lista de roles de usuario
  noRolesAvailable = false; // bandera para mostrar mensaje si no hay roles

  form!: FormGroup;
  rolUserId!: number;

  ngOnInit(): void {
    this.rolService.getAll().subscribe((data) => {
      this.roles = data;
      if (this.roles.length === 0) {
        this.noRolesAvailable = true;
        this.form.disable();
      }
    });

    this.rolUserId = Number(this.route.snapshot.paramMap.get('id'));

    this.form = this.fb.group({
      id: [this.rolUserId],
      userId: [0], // importante para el update
      roleId: [0], // importante para el update
      active: [true, Validators.required],
    });

    this.rolUserService.getById(this.rolUserId).subscribe(rolUser => {
      this.form.patchValue({
        userId: rolUser.userId,
        roleId: rolUser.roleId,
        active: rolUser.active,
      });
    });

    this.rolUserService.getAll().subscribe((data) => {
      this.rolUsers = data;
    });
  }

  submit(): void {
    if (this.form.invalid) return;

    const payload = {
      ...this.form.value,
      // active: this.form.value['active'] === 'true',
    };
    const userId = this.form.value.userId;
    const roleId = this.form.value.roleId;

    const alreadyExists = this.rolUsers.some(ru => ru.userId === userId && ru.roleId === roleId);

    if (alreadyExists) {
      alert('No se puede actualizar el usuario con este rol, elige otro');
      return;
    }
    this.rolUserService.update(payload).subscribe({
      next: () => this.router.navigate(['/rol-user']),
      error: err => alert('Error al actualizar: ' + err.message),
    });
  }

  cancelar(): void {
    this.router.navigate(['/rol-user']);
  }

}
