import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment.development'; // Asegúrate de que la URL base esté en el entorno

@Injectable({
  providedIn: 'root'
})
export class RolFormPermissionService {

  private apiUrl = `${environment.apiURL}api/RolFormPermission`; // Cambia esta URL de acuerdo con tu configuración

  constructor(private http: HttpClient) { }

  // Obtener todos los permisos de rol
  getAll(): Observable<any> {
    return this.http.get<any>(`${this.apiUrl}`);
  }

  // Obtener permiso de rol por ID
  getById(id: number): Observable<any> {
    return this.http.get<any>(`${this.apiUrl}/${id}`);
  }

  // Crear un nuevo permiso de rol
  create(rolFormPermission: any): Observable<any> {
    return this.http.post<any>(`${this.apiUrl}`, rolFormPermission);
  }

  // Actualizar un permiso de rol
  update(rolFormPermission: any): Observable<any> {
    return this.http.put<any>(`${this.apiUrl}/update`, rolFormPermission);
  }

  // Eliminar un permiso de rol
  delete(id: number): Observable<any> {
    return this.http.delete<any>(`${this.apiUrl}/${id}`);
  }

  // Desactivar un permiso de rol (eliminación lógica)
  logicalDelete(id: number): Observable<any> {
    return this.http.patch<any>(`${this.apiUrl}/Logical/${id}`, {});
  }
}
