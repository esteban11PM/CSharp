import { CommonModule } from '@angular/common';
import { Component, inject, OnInit } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatIconModule } from '@angular/material/icon';
import { MatTableModule } from '@angular/material/table';
import { RouterLink } from '@angular/router';
import { RolFormPermissionService } from '../../services/rol-form-permission.service';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-indice-rol-form-permission',
  standalone: true,
  imports: [MatCardModule, MatTableModule, MatIconModule, MatButtonModule, CommonModule, RouterLink],
  templateUrl: './indice-rol-form-permission.component.html',
  styleUrl: './indice-rol-form-permission.component.css'
})
export class IndiceRolFormPermissionComponent implements OnInit {
  
  private rolFormPermissionService = inject(RolFormPermissionService);
  rolFormPermissions: any[] = [];
  columnas = ['id', 'rolename', 'pemissionname', 'formname' ,'active', 'acciones'];

  ngOnInit(): void {
    this.cargarRolFormPermissions();
  }
  cargarRolFormPermissions(): void {
    this.rolFormPermissionService.getAll().subscribe({
      next: data => this.rolFormPermissions = data,
      error: err => console.error("Error cargando permisos de rol", err)
    });
  }

  eliminarRolFormPermission(rolFormPermission: any) {
    Swal.fire({
      title: '¿Qué tipo de eliminación deseas?',
      text: `Permiso de rol: ${rolFormPermission.permissionname}`,
      icon: 'warning',
      showCancelButton: true,
      showDenyButton: true,
      confirmButtonText: 'Lógica',
      denyButtonText: 'Permanente',
      cancelButtonText: 'Cancelar',
      confirmButtonColor: '#3085d6',
      denyButtonColor: '#d33',
    }).then(result => {
      if (result.isConfirmed) {
        this.rolFormPermissionService.logicalDelete(rolFormPermission.id).subscribe(() => {
          Swal.fire('Eliminado lógicamente', '', 'success');
          this.cargarRolFormPermissions();
        });
      } else if (result.isDenied) {
        this.rolFormPermissionService.delete(rolFormPermission.id).subscribe(() => {
          Swal.fire('Eliminado permanentemente', '', 'success');
          this.cargarRolFormPermissions();
        });
      }
    });
  }
}
