// src/app/services/user-rol.service.ts

import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment.development';

// valida que el archivo de entorno existe
export interface RolUserDTO {
  userId: any;
  roleId: any;
  id?: number;
  UserName: string;
  RoleName: string;
  active?: boolean;
}

// para saber que datos enviar al servidor
export interface RolUserCreateDTO {
  id?: number;
  userId: number;
  roleId: number;
}

@Injectable({
  providedIn: 'root'
})
export class rolUserService {

  private apiUrl = `${environment.apiURL}api/UserRol`;

  constructor(private http: HttpClient) { }

  // Listar todos los registros
  getAll(): Observable<RolUserDTO[]> {
    return this.http.get<RolUserDTO[]>(this.apiUrl);
  }

  // Obtener un registro por ID
  getById(id: number): Observable<RolUserDTO> {
    return this.http.get<RolUserDTO>(`${this.apiUrl}/${id}`);
  }

  // Crear un nuevo registro (usa RolUserCreateDTO)
  create(rolUser: any): Observable<any> {
    return this.http.post<RolUserDTO>(this.apiUrl, rolUser);
  }

  // Actualizar un registro existente (usa RolUserCreateDTO)
  update(payload: any): Observable<any> {
    return this.http.put<any>(`${this.apiUrl}/update`, payload);
  }

  // Eliminación física
  delete(id: number): Observable<any> {
    return this.http.delete<any>(`${this.apiUrl}/${id}`);
  }

  // Eliminación lógica (soft delete)
  logicalDelete(id: number): Observable<any> {
    return this.http.patch<any>(`${this.apiUrl}/Logical/${id}`, {});
  }
}
