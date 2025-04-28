import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { ActivatedRoute, Router } from '@angular/router';
import { FormService } from '../../services/form.service';

@Component({
  selector: 'app-update-form',
  standalone: true,
  imports: [
    ReactiveFormsModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    MatSelectModule,
    CommonModule
  ],
  templateUrl: './update-form.component.html',
  styleUrl: './update-form.component.css'
})
export class UpdateFormComponent implements OnInit {
  form!: FormGroup;
  formId!: number;

  constructor(
    private fb: FormBuilder,
    private formService: FormService,
    private route: ActivatedRoute,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.formId = Number(this.route.snapshot.paramMap.get('id'));
    this.form = this.fb.group({
      id: [this.formId],
      name: ['', Validators.required],
      description: [''],
      active: [true, Validators.required],
    });

    this.formService.getById(this.formId).subscribe(form => {
      this.form.patchValue({
        name: form.name,
        description: form.description,
        active: form.active,
      });
    });
  }

  submit(): void {
    if (this.form.invalid) return;

    const payload = {
      ...this.form.value,
      active: this.form.value['active'] === 'true',
    };

    this.formService.update(payload).subscribe({
      next: () => {
        this.router.navigate(['/form']);
      },
      error: (error) => {
        console.error('Error updating form:', error);
        alert('Error updating form');
      }
    });
  }

  cancelar(): void {
    this.router.navigate(['/form']);
  }

}
