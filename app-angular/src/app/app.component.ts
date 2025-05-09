import { Component, inject } from '@angular/core';
import { MenuComponent } from "./menu/menu.component";
import { Router, RouterOutlet } from '@angular/router';
import { NgIf } from '@angular/common';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [MenuComponent, RouterOutlet, NgIf],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent {

  private router = inject(Router);

  get mostrarMenu(): boolean {
    const rutasOcultas = ['/login', '/register'];
    return !rutasOcultas.some(r => this.router.url.startsWith(r));
  }

}