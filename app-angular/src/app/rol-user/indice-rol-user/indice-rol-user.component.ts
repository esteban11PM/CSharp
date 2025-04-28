import { CommonModule } from '@angular/common';
import { rolUserService } from '../../services/rol-user.service';
import { Component, inject, OnInit } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatIconModule } from '@angular/material/icon';
import { MatTableModule } from '@angular/material/table';
import { RouterLink } from '@angular/router';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-indice-rol-user',
  standalone: true,
  imports: [MatCardModule, MatTableModule, MatIconModule, MatButtonModule, CommonModule, RouterLink],
  templateUrl: './indice-rol-user.component.html',
  styleUrl: './indice-rol-user.component.css'
})
export class IndiceRolUserComponent implements OnInit {

  private rolUserService = inject(rolUserService);
  rolUsers: any[] = [];
  columnas = ['id', 'username', 'rolename', 'active', 'acciones'];

  ngOnInit(): void {
    this.cargarRolUsers();
  }

  cargarRolUsers(): void {
    this.rolUserService.getAll().subscribe({
      next: data => this.rolUsers = data,
      error: err => console.error("Error cargando rol-usuarios", err)
    });
  }

  eliminarRolUser(rolUser: any) {
    Swal.fire({
      title: '¿Qué tipo de eliminación deseas?',
      text: `Rol-Usuario: ${rolUser.rolname}`,
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
        this.rolUserService.logicalDelete(rolUser.id).subscribe(() => {
          Swal.fire('Eliminado lógicamente', '', 'success');
          this.cargarRolUsers();
        });
      } else if (result.isDenied) {
        this.rolUserService.delete(rolUser.id).subscribe(() => {
          Swal.fire('Eliminado permanentemente', '', 'success');
          this.cargarRolUsers();
        });
      }
    });
  }
}
