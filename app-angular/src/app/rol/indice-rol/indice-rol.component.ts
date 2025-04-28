import { Component, inject, OnInit } from '@angular/core';
import { RolService } from '../../services/rol.service';
import { MatCardModule } from '@angular/material/card';
import { MatTableModule } from '@angular/material/table';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import Swal from 'sweetalert2';
@Component({
  selector: 'app-indice-rol',
  standalone: true,
  imports: [MatCardModule, MatTableModule, MatIconModule, MatButtonModule, CommonModule, RouterLink],
  templateUrl: './indice-rol.component.html',
  styleUrl: './indice-rol.component.css'
})
export class IndiceRolComponent implements OnInit {
  private rolService = inject(RolService);
  roles: any[] = [];
  columnas = ['id', 'name', 'description', 'state', 'acciones'];
  ngOnInit(): void {
    this.cargarRoles();
  }
  cargarRoles(): void {
    this.rolService.getAll().subscribe({
      next: data => this.roles = data,
      error: err => console.error("Error cargando roles", err)
    });
  }
  eliminarRol(rol: any) {
    Swal.fire({
      title: '¿Qué tipo de eliminación deseas?',
      text: `Rol: ${rol.name}`,
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
        this.rolService.logicalDelete(rol.id).subscribe(() => {
          Swal.fire('Eliminado lógicamente', '', 'success');
          this.cargarRoles();
        });
      } else if (result.isDenied) {
        this.rolService.delete(rol.id).subscribe(() => {
          Swal.fire('Eliminado permanentemente', '', 'success');
          this.cargarRoles();
        });
      }
    });
  }
}
