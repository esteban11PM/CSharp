import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../environments/environment.development'; // Asegúrate de que este import apunte a tu archivo environment correcto
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class RolService {

  private apiUrl = `${environment.apiURL}api/Rol`;

  constructor(private http: HttpClient) { }

  // Obtener todos los roles
  getAll(): Observable<any[]> {
    return this.http.get<any[]>(this.apiUrl);
  }

  // Obtener un rol por ID
  getById(id: number): Observable<any> {
    return this.http.get<any>(`${this.apiUrl}/${id}`);
  }

  // Crear un nuevo rol
  create(rol: any): Observable<any> {
    return this.http.post<any>(this.apiUrl, rol);
  }

  // Actualizar un rol
  update(rol: any): Observable<any> {
    return this.http.put<any>(`${this.apiUrl}/update`, rol);
  }

  // Eliminar un rol (eliminación persistente)
  delete(id: number): Observable<any> {
    return this.http.delete<any>(`${this.apiUrl}/${id}`);
  }

  // Desactivar un rol (eliminación lógica)
  logicalDelete(id: number): Observable<any> {
    return this.http.patch<any>(`${this.apiUrl}/Logical/${id}`, null);
  }
}