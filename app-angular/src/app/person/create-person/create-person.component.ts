import { NgFor, NgIf } from '@angular/common';
import { Component, inject } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatFormField } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { Router } from '@angular/router';
import { PersonService } from '../../services/person.service';
import { MatSelectModule } from '@angular/material/select';

@Component({
  selector: 'app-create-person',
  standalone: true,
  imports: [ReactiveFormsModule, MatFormField, MatInputModule, MatButtonModule, MatSelectModule, NgIf, NgFor],
  templateUrl: './create-person.component.html',
  styleUrl: './create-person.component.css'
})
export class CreatePersonComponent {

  private formBuilder = inject(FormBuilder);
  private personService = inject(PersonService);
  private router = inject(Router);

  form = this.formBuilder.group({
    name: ['', Validators.required],
    lastName: ['', Validators.required],
    email: ['', [Validators.required, Validators.email]],
    documentType: ['', Validators.required],
    documentNumber: ['', [Validators.required, Validators.pattern(/^\d{8,10}$/)]],
    phone: ['', [Validators.required, Validators.pattern(/^\d{10}$/)]],
    address: [''],
    blodType: ['', Validators.required],
    status: [true, Validators.required],
  });
  submit(): void {
    if (this.form.invalid) return;

    this.personService.create(this.form.value).subscribe({
      next: () => this.router.navigate(['/person']),
      error: err => alert("Error al registrar: " + err.message)
    });
  }
  cancelar(): void {
    this.router.navigate(['/person']);
  }
  // Validar que solo se introduzcan números
  onlyNumbers(event: KeyboardEvent) {
    const charCode = event.charCode;
    if (charCode < 48 || charCode > 57) {
      event.preventDefault();
    }
  }
}
