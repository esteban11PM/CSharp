import { Component, inject, OnInit } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { UserService } from '../../services/user.service';
import { MatFormFieldModule, MatLabel, MatHint } from '@angular/material/form-field';
import { MatSelectModule } from '@angular/material/select';
import { MatButtonModule } from '@angular/material/button';
import { MatInputModule } from '@angular/material/input';
import { NgFor, NgIf } from '@angular/common';
import { PersonService } from '../../services/person.service';

@Component({
  selector: 'app-form-user',
  standalone: true,
  imports: [ReactiveFormsModule, MatFormFieldModule, MatInputModule, MatSelectModule, MatButtonModule, NgFor, NgIf],
  templateUrl: './create-user.component.html',
  styleUrl: './create-user.component.css'
})
export class FormUserComponent implements OnInit {

  private formBuilder = inject(FormBuilder);
  private userService = inject(UserService);
  private personService = inject(PersonService)
  private router = inject(Router);

  persons: any[] = [];
  noPersonsAvailable = false;

  form = this.formBuilder.group({
    username: ['', Validators.required],
    password: ['', Validators.required],
    personId: [null, Validators.required],
    state: [true, Validators.required],
    
  });

  ngOnInit(): void {
    this.personService.getAvailable().subscribe((data) => {
      this.persons = data;
      if (this.persons.length === 0) {
        this.noPersonsAvailable = true;
        this.form.disable(); 
      }
    });
  }

  submit(): void {
    if (this.form.invalid) return;

    this.userService.create(this.form.value).subscribe({
      next: () => this.router.navigate(['/user']),
      error: err => alert("Error al registrar: " + err.message)
    });
  }

  cancelar(): void {
    this.router.navigate(['/user']);
  }

}
