import { CommonModule } from '@angular/common';
import { Component, inject, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { ActivatedRoute, Router } from '@angular/router';
import { PersonService } from '../../services/person.service';

@Component({
  selector: 'app-update-person',
  standalone: true,
  imports: [
    ReactiveFormsModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    MatSelectModule,
    CommonModule,
  ],
  templateUrl: './update-person.component.html',
  styleUrl: './update-person.component.css'
})
export class UpdatePersonComponent implements OnInit {
  private fb = inject(FormBuilder);
  private personService = inject(PersonService);
  private route = inject(ActivatedRoute);
  private router = inject(Router);

  form!: FormGroup;
  personId!: number;

  ngOnInit(): void {
    this.personId = Number(this.route.snapshot.paramMap.get('id'));
    this.form = this.fb.group({
      id: [0], // importante para el update
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

    this.personService.getById(this.personId).subscribe(person => {
      this.form.patchValue({
        id: person.id,
        name: person.name,
        lastName: person.lastName,
        email: person.email,
        documentType: person.documentType,
        documentNumber: person.documentNumber,
        phone: person.phone,
        address: person.address,
        blodType: person.blodType,
        status: person.status,
      });
    });
  }

  submit(): void {
    if (this.form.invalid) return;

    const payload = {
      ...this.form.value,
      status: this.form.value['status'] === 'true',
    };
  
    this.personService.update(payload).subscribe({
      next: () => this.router.navigate(['/person']),
      error: err => alert('Error al actualizar: ' + err.message),
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
