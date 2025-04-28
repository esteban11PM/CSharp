import { Component, inject, OnInit } from '@angular/core';
import { ModelService } from '../../services/model.service'; // Asegúrate de que este servicio exista
import { MatCardModule } from '@angular/material/card';
import { MatTableModule } from '@angular/material/table';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-indice-module',
  standalone: true,
  imports: [MatCardModule, MatTableModule, MatIconModule, MatButtonModule, CommonModule, RouterLink],
  templateUrl: './indice-module.component.html',
  styleUrl: './indice-module.component.css'
})
export class IndiceModuleComponent implements OnInit {
  private moduleService = inject(ModelService);
  modulos: any[] = [];
  columnas = ['id', 'name', 'description', 'state', 'acciones'];

  ngOnInit(): void {
    this.cargarModulos();
  }

  cargarModulos(): void {
    this.moduleService.getAll().subscribe({
      next: data => this.modulos = data,
      error: err => console.error("Error cargando módulos", err)
    });
  }

  eliminarModulo(mod: any) {
    Swal.fire({
      title: '¿Qué tipo de eliminación deseas?',
      text: `Módulo: ${mod.name}`,
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
        this.moduleService.logicalDelete(mod.id).subscribe(() => {
          Swal.fire('Eliminado lógicamente', '', 'success');
          this.cargarModulos();
        });
      } else if (result.isDenied) {
        this.moduleService.delete(mod.id).subscribe(() => {
          Swal.fire('Eliminado permanentemente', '', 'success');
          this.cargarModulos();
        });
      }
    });
  }
}
