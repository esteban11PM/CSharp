import { NgFor, NgIf } from '@angular/common';
import { Component, inject, OnInit } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { Router } from '@angular/router';
import { rolUserService } from '../../services/rol-user.service';
import { RolService } from '../../services/rol.service';
import { UserService } from '../../services/user.service';

@Component({
  selector: 'app-create-rol-user',
  standalone: true,
  imports: [ReactiveFormsModule, MatFormFieldModule, MatInputModule, MatSelectModule, MatButtonModule, NgFor, NgIf],
  templateUrl: './create-rol-user.component.html',
  styleUrl: './create-rol-user.component.css'
})
export class CreateRolUserComponent implements OnInit {

  private formBuilder = inject(FormBuilder);
  private rolUserService = inject(rolUserService);
  private rolService = inject(RolService);
  private userService = inject(UserService);
  private router = inject(Router);

  roles: any[] = [];
  users: any[] = [];
  rolUsers: any[] = [];
  noRolesAvailable = false;
  noUsersAvailable = false;

  form = this.formBuilder.group({
    userId: [null, Validators.required],
    roleId: [null, Validators.required],
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

    this.userService.getAll().subscribe((data) => {
      this.users = data;
      if (this.users.length === 0) {
        this.noUsersAvailable = true;
        this.form.disable();
      }
    });

    this.rolUserService.getAll().subscribe((data) => {
      this.rolUsers = data;
    });
  }
  submit(): void {
    if (this.form.invalid) return;

    const userId = this.form.value.userId;
    const roleId = this.form.value.roleId;

    const alreadyExists = this.rolUsers.some(ru => ru.userId === userId && ru.roleId === roleId);

    if (alreadyExists) {
      alert('El usuario ya tiene asignado este ROL, elige otro');
      return;
    }

    this.rolUserService.create(this.form.value).subscribe({
      next: () => this.router.navigate(['/rol-user']),
      error: err => alert("Error al registrar: " + err.message)
    });
  }
  cancelar(): void {
    this.router.navigate(['/rol-user']);
  }

}
