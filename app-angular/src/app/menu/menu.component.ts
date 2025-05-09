import { Component, inject } from '@angular/core';
import {MatIconModule} from '@angular/material/icon';
import {MatButtonModule} from '@angular/material/button';
import {MatToolbarModule} from '@angular/material/toolbar';
import { Router, RouterLink } from '@angular/router';
import { AccesoService } from '../services/acceso.service';

@Component({
  selector: 'app-menu',
  standalone: true,
  imports: [MatToolbarModule, MatButtonModule, MatIconModule, RouterLink],
  templateUrl: './menu.component.html',
  styleUrl: './menu.component.css'
})
export class MenuComponent {
  private AccessoService = inject(AccesoService);
  private router = inject(Router);

  logout(): void {
    this.AccessoService.logout();
    this.router.navigate(['/login']);
  }

}
