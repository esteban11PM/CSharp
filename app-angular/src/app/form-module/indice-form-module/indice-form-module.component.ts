import { CommonModule } from '@angular/common';
import { Component, inject, OnInit } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatIconModule } from '@angular/material/icon';
import { MatTableModule } from '@angular/material/table';
import { RouterLink } from '@angular/router';
import Swal from 'sweetalert2';
import { FormModuleService } from '../../services/form-module.service';

@Component({
  selector: 'app-indice-form-module',
  standalone: true,
  imports: [MatCardModule, MatTableModule, MatIconModule, MatButtonModule, CommonModule, RouterLink],
  templateUrl: './indice-form-module.component.html',
  styleUrl: './indice-form-module.component.css'
})
export class IndiceFormModuleComponent implements OnInit {

  private formModuleService = inject(FormModuleService);
  formModules: any[] = [];
  columnas = ['id', 'formname', 'modulename', 'active', 'acciones'];

  ngOnInit(): void {
    this.cargarFormModules();
  }

  cargarFormModules(): void {
    this.formModuleService.getAll().subscribe({
      next: data => this.formModules = data,
      error: err => console.error("Error cargando form-module", err)
    });
  }

  eliminarFormModule(formModule: any) {
    Swal.fire({
      title: '¿Qué tipo de eliminación deseas?',
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
        this.formModuleService.logicalDelete(formModule.id).subscribe(() => {
          Swal.fire('Eliminado lógicamente', '', 'success');
          this.cargarFormModules();
        });
      } else if (result.isDenied) {
        this.formModuleService.delete(formModule.id).subscribe(() => {
          Swal.fire('Eliminado permanentemente', '', 'success');
          this.cargarFormModules();
        });
      }
    });
  }

}
