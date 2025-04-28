import { Component, inject } from '@angular/core';
import { PruebaConextionService } from '../prueba-conextion.service';

@Component({
  selector: 'app-landing',
  imports: [],
  templateUrl: './landing-component.component.html',
  styleUrl: './landing-component.component.css'
})
export class LandingComponent {
  pruebaConexion = inject(PruebaConextionService);
  users: any[] = [];

  constructor(){
    this.pruebaConexion.obtenerUsers().subscribe(datos => {
      this.users = datos;
    })
  }
}
