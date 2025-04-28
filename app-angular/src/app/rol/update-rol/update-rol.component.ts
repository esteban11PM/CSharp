import { CommonModule } from '@angular/common';
import { Component, inject, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { ActivatedRoute, Router } from '@angular/router';
import { RolService } from '../../services/rol.service';

@Component({
  selector: 'app-update-rol',
  imports: [
    ReactiveFormsModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    MatSelectModule,
    CommonModule
  ],
  templateUrl: './update-rol.component.html',
  styleUrl: './update-rol.component.css',
  standalone: true,
})
export class UpdateRolComponent implements OnInit {
  private fb = inject(FormBuilder);
  private rolService = inject(RolService);
  private route = inject(ActivatedRoute);
  private router = inject(Router);
  form!: FormGroup;
  rolId!: number;
  
  ngOnInit(): void {
    this.rolId = Number(this.route.snapshot.paramMap.get('id'));
    this.form = this.fb.group({
      id: [this.rolId],
      name: ['', Validators.required],
      description: [''],
      active: [true, Validators.required],
    });

    this.rolService.getById(this.rolId).subscribe(rol => {
      this.form.patchValue({
        name: rol.name,
        description: rol.description,
        active: rol.active,
      });
    });
  }

  submit(): void {
    if (this.form.invalid) return;

    const payload = {
      ...this.form.value,
      active: this.form.value['active'] === 'true',
    };

    this.rolService.update(payload).subscribe({
      next: () => {
        this.router.navigate(['/rol']);
      },
      error: err => {
        console.error(err);
      },
    });
  }

  cancelar(): void {
    this.router.navigate(['/rol']);
  }

}
