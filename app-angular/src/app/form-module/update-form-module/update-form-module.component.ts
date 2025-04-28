import { NgForOf } from '@angular/common';
import { Component, inject, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { Router, ActivatedRoute } from '@angular/router';
import { FormModuleService } from '../../services/form-module.service';
import { FormService } from '../../services/form.service';

@Component({
  selector: 'app-update-form-module',
  standalone: true,
  imports: [ReactiveFormsModule, MatFormFieldModule, MatInputModule, MatSelectModule, MatButtonModule, NgForOf],
  templateUrl: './update-form-module.component.html',
  styleUrl: './update-form-module.component.css'
})
export class UpdateFormModuleComponent implements OnInit {

  private fb = inject(FormBuilder);
  private formModuleService = inject(FormModuleService);
  private formService = inject(FormService);
  private router = inject(Router);
  private route = inject(ActivatedRoute);

  formModules: any[] = []; // lista de módulos de formulario
  forms: any[] = []; // lista de roles de usuario
  noFormModulesAvailable = false;// bandera para mostrar mensaje si no hay roles

  form!: FormGroup;
  formModuleId!: number;

  ngOnInit(): void {
    this.formModuleId = Number(this.route.snapshot.paramMap.get('id'));

    this.form = this.fb.group({
      id: [this.formModuleId],
      moduleId: [0], // importante para el update
      formId: [0], // importante para el update
      active: [true, Validators.required],
    });

    this.formService.getAll().subscribe((data) => {
      this.forms = data;
      if (this.forms.length === 0) {
        this.noFormModulesAvailable = true;
        this.form.disable();
      }
    });

    this.formModuleService.getById(this.formModuleId).subscribe(formModule => {
      this.form.patchValue({
        moduleId: formModule.moduleId,
        formId: formModule.formId,
        active: formModule.active,
      });
    });
    this.formModuleService.getAll().subscribe((data) => {
      this.formModules = data;
    });
  }

  submit(): void {
    if (this.form.invalid) return;

    const payload = {
      ...this.form.value,
      // active: this.form.value['active'] === 'true',
    };
    const moduleId = this.form.value.moduleId;
    const formId = this.form.value.formId;

    const alreadyExists = this.formModules.some(fm => fm.moduleId === moduleId && fm.formId === formId);

    if (alreadyExists) {
      alert('No se puede actualizar el formulario con este modulo, elige otro');
      return;
    }
    this.formModuleService.update(payload).subscribe({
      next: () => this.router.navigate(['/form-module']),
      error: err => alert('Error al actualizar: ' + err.message),
    });
  }

  cancelar(): void {
    this.router.navigate(['/form-module']);
  }
}
