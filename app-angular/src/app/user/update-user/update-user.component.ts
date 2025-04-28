import { Component, inject, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatButtonModule } from '@angular/material/button';
import { UserService } from '../../services/user.service';

@Component({
  selector: 'app-update-user',
  standalone: true,
  imports: [ReactiveFormsModule, MatFormFieldModule, MatInputModule, MatSelectModule, MatButtonModule],
  templateUrl: './update-user.component.html',
  styleUrl: './update-user.component.css'
})
export class UpdateUserComponent implements OnInit{
  private fb = inject(FormBuilder);
  private userService = inject(UserService);
  private route = inject(ActivatedRoute);
  private router = inject(Router);

  form!: FormGroup;
  userId!: number;

  ngOnInit(): void {
    this.userId = Number(this.route.snapshot.paramMap.get('id'));
    this.form = this.fb.group({
      id: [this.userId],
      username: ['', Validators.required],
      password: ['', Validators.required],
      personId: [0], // precargado manual
      state: [true, Validators.required],
    });

    this.userService.getById(this.userId).subscribe(user => {
      this.form.patchValue({
        username: user.username,
        password: '',
        personId: user.personId,
        state: user.state,
      });
    });
  }

  submit(): void {
    if (this.form.invalid) return;

    const payload = {
      ...this.form.value,
      state: this.form.value['state'] === 'true',
    };

    this.userService.update(payload).subscribe({
      next: () => this.router.navigate(['/user']),
      error: err => alert('Error al actualizar: ' + err.message),
    });
  }

  cancelar(): void {
    this.router.navigate(['/user']);
  }
}
