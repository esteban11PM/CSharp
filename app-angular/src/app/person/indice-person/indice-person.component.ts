import { CommonModule } from '@angular/common';
import { Component, inject, OnInit } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatIconModule } from '@angular/material/icon';
import { MatTableModule } from '@angular/material/table';
import { RouterLink } from '@angular/router';
import Swal from 'sweetalert2';
import { PersonService } from '../../services/person.service';

@Component({
  selector: 'app-indice-person',
  standalone: true,
  imports: [MatCardModule, MatTableModule, MatIconModule, MatButtonModule, CommonModule, RouterLink],
  templateUrl: './indice-person.component.html',
  styleUrl: './indice-person.component.css'
})
export class IndicePersonComponent implements OnInit {
  private personService = inject(PersonService);
  columnas = ['id', 'name', 'lastName', 'email', 'documentType', 'documentNumber', 'phone', 'address', 'blodType', 'state', 'acciones'];
  personas: any[] = [];

  ngOnInit(): void {
    this.cargarPersonas();
  }

  cargarPersonas(): void {
    this.personService.getAll().subscribe({
      next: data => this.personas = data,
      error: err => console.error("Error cargando personas", err)
    });
  }

  eliminarPersona(persona: any) {
    Swal.fire({
      title: '¿Qué tipo de eliminación deseas?',
      text: `Persona: ${persona.name}`,
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
        this.personService.logicalDelete(persona.id).subscribe(() => {
          Swal.fire('Eliminado lógicamente', '', 'success');
          this.cargarPersonas();
        });
      } else if (result.isDenied) {
        this.personService.delete(persona.id).subscribe(() => {
          Swal.fire('Eliminado permanentemente', '', 'success');
          this.cargarPersonas();
        });
      }
    });
  }
}
