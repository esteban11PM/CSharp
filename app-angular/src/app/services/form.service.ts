import { inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../environments/environment.development'; // Verifica que esté apuntando al environment correcto
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class FormService {
  private http = inject(HttpClient);
  private apiUrl = `${environment.apiURL}api/Form/`;

  // Obtener todos los formularios
  getAll(): Observable<any[]> {
    return this.http.get<any[]>(this.apiUrl);
  }

  // Obtener un formulario por ID
  getById(id: number): Observable<any> {
    return this.http.get<any>(`${this.apiUrl}${id}`);
  }

  // Crear un nuevo formulario
  create(form: any): Observable<any> {
    return this.http.post<any>(this.apiUrl, form);
  }

  // Actualizar un formulario
  update(form: any): Observable<any> {
    return this.http.put<any>(`${this.apiUrl}update`, form);
  }

  // Eliminar un formulario (eliminación persistente)
  delete(id: number): Observable<any> {
    return this.http.delete<any>(`${this.apiUrl}${id}`);
  }

  // Desactivar un formulario (eliminación lógica)
  logicalDelete(id: number): Observable<any> {
    return this.http.patch<any>(`${this.apiUrl}Logical/${id}`, {});
  }
}

