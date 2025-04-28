import { Component, inject, OnInit } from '@angular/core';
import { UserService } from '../../services/user.service';
import { MatCardModule } from '@angular/material/card';
import { MatTableModule } from '@angular/material/table';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-indice-user',
  standalone: true,
  imports: [MatCardModule, MatTableModule, MatIconModule, MatButtonModule, CommonModule, RouterLink],
  templateUrl: './indice-user.component.html',
  styleUrl: './indice-user.component.css'
})
export class IndiceUserComponent implements OnInit {
  private userService = inject(UserService);
  usuarios: any[] = [];
  columnas = ['id', 'username', 'password', 'personName', 'acciones'];

  ngOnInit(): void {
    this.cargarUsuarios();
  }


  cargarUsuarios(): void {
    this.userService.getAll().subscribe({
      next: data => this.usuarios = data,
      error: err => console.error("Error cargando usuarios", err)
    });
  }

  eliminarUsuario(user: any) {
    Swal.fire({
      title: '¿Qué tipo de eliminación deseas?',
      text: `Usuario: ${user.username}`,
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
        this.userService.logicalDelete(user.id).subscribe(() => {
          Swal.fire('Eliminado lógicamente', '', 'success');
          this.cargarUsuarios();
        });
      } else if (result.isDenied) {
        this.userService.delete(user.id).subscribe(() => {
          Swal.fire('Eliminado permanentemente', '', 'success');
          this.cargarUsuarios();
        });
      }
    });
  }
}
