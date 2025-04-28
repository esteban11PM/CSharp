import { CommonModule } from '@angular/common';
import { Component, inject, OnInit } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatIconModule } from '@angular/material/icon';
import { MatTableModule } from '@angular/material/table';
import { RouterLink } from '@angular/router';
import { PermissionService } from '../../services/permission.service';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-indice-permission',
  standalone: true,
  imports: [MatCardModule, MatTableModule, MatIconModule, MatButtonModule, CommonModule, RouterLink],
  templateUrl: './indice-permission.component.html',
  styleUrl: './indice-permission.component.css'
})
export class IndicePermissionComponent implements OnInit {
  private permissionService = inject(PermissionService);
  permissions: any[] = [];
  columnas = ['id', 'name', 'description', 'state', 'acciones'];

  ngOnInit(): void {
    this.cargarPermisos();
  }

  cargarPermisos(): void {
    this.permissionService.getAll().subscribe({
      next: data => this.permissions = data,
      error: err => console.error("Error cargando permisos", err)
    });
  }

  eliminarPermiso(permiso: any) {
    Swal.fire({
      title: '¿Qué tipo de eliminación deseas?',
      text: `Permiso: ${permiso.name}`,
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
        this.permissionService.logicalDelete(permiso.id).subscribe(() => {
          Swal.fire('Eliminado lógicamente', '', 'success');
          this.cargarPermisos();
        });
      } else if (result.isDenied) {
        this.permissionService.delete(permiso.id).subscribe(() => {
          Swal.fire('Eliminado permanentemente', '', 'success');
          this.cargarPermisos();
        });
      }
    });
  }
}
