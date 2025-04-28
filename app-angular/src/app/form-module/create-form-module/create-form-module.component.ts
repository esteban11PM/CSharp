import { NgFor, NgIf } from '@angular/common';
import { Component, inject, OnInit } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { Router } from '@angular/router';
import { FormModuleService } from '../../services/form-module.service';
import { FormService } from '../../services/form.service';
import { ModelService } from '../../services/model.service';
@Component({
  selector: 'app-create-form-module',
  standalone: true,
  imports: [ReactiveFormsModule, MatFormFieldModule, MatInputModule, MatSelectModule, MatButtonModule, NgFor, NgIf],
  templateUrl: './create-form-module.component.html',
  styleUrl: './create-form-module.component.css'
})
export class CreateFormModuleComponent implements OnInit {

  private formBuilder = inject(FormBuilder);
  private formModuleService = inject(FormModuleService);
  private formService = inject(FormService);
  private moduleService = inject(ModelService);
  private router = inject(Router);
  
  modules: any[] = [];
  forms: any[] = [];
  formModules: any[] = [];
  noModulesAvailable = false;
  noFormsAvailable = false;

  form = this.formBuilder.group({
    moduleId: [null, Validators.required],
    formId: [null, Validators.required],
    active: [true, Validators.required],
  });

  ngOnInit(): void {
    this.formService.getAll().subscribe((data) => {
      this.forms = data;
      if (this.forms.length === 0) {
        this.noFormsAvailable = true;
        this.form.disable();
      }
    });

    this.moduleService.getAll().subscribe((data) => {
      this.modules = data;
      if (this.modules.length === 0) {
        this.noModulesAvailable = true;
        this.form.disable();
      }
    });

    this.formModuleService.getAll().subscribe((data) => {
      this.formModules = data;
    });
  }

  submit(): void {
    if (this.form.invalid) return;

    const moduleId = this.form.value.moduleId;
    const formId = this.form.value.formId;

    const alreadyExists = this.formModules.some(fm => fm.moduleId === moduleId && fm.formId === formId);

    if (alreadyExists) {
      alert('El formulario ya esta registrado en este modulo, elige otro');
      return;
    }

    this.formModuleService.create(this.form.value).subscribe({
      next: () => this.router.navigate(['/form-module']),
      error: err => alert("Error al registrar: " + err.message)
    });
  }

  cancelar(): void {
    this.router.navigate(['/form-module']);
  }

}
