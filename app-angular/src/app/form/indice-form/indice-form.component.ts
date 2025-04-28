import { CommonModule } from '@angular/common';
import { Component, inject, OnInit } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatIconModule } from '@angular/material/icon';
import { MatTableModule } from '@angular/material/table';
import { RouterLink } from '@angular/router';
import { FormService } from '../../services/form.service';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-indice-form',
  imports: [MatCardModule, MatTableModule, MatIconModule, MatButtonModule, CommonModule, RouterLink],
  templateUrl: './indice-form.component.html',
  styleUrl: './indice-form.component.css',
  standalone: true
})
export class IndiceFormComponent implements OnInit {
  private FormService = inject(FormService);
  columnas = ['id', 'name', 'description', 'state', 'acciones'];
  forms: any[] = [];

  ngOnInit(): void {
    this.cargarFormularios();
  }

  cargarFormularios(): void {
    this.FormService.getAll().subscribe({
      next: data => this.forms = data,
      error: err => console.error("Error cargando formularios", err)
    });
  }

  eliminarFormulario(form: any) {
    Swal.fire({
      title: '¿Qué tipo de eliminación deseas?',
      text: `Formulario: ${form.name}`,
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
        this.FormService.logicalDelete(form.id).subscribe(() => {
          Swal.fire('Eliminado lógicamente', '', 'success');
          this.cargarFormularios();
        });
      } else if (result.isDenied) {
        this.FormService.delete(form.id).subscribe(() => {
          Swal.fire('Eliminado permanentemente', '', 'success');
          this.cargarFormularios();
        });
      }
    });
  }

}
