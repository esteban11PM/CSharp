import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment.development';

@Injectable({
  providedIn: 'root'
})
export class PermissionService {
  
  private apiUrl = `${environment.apiURL}api/Permission/`;

  constructor(private http: HttpClient) {}

  getAll(): Observable<any[]> {
    return this.http.get<any[]>(this.apiUrl);
  }

  getById(id: number): Observable<any> {
    return this.http.get<any>(`${this.apiUrl}${id}`);
  }

  create(permission: any): Observable<any> {
    return this.http.post<any>(this.apiUrl, permission);
  }

  update(permission: any): Observable<any> {
    return this.http.put<any>(`${this.apiUrl}update`, permission);
  }

  delete(id: number): Observable<any> {
    return this.http.delete<any>(`${this.apiUrl}${id}`);
  }

  logicalDelete(id: number): Observable<any> {
    return this.http.patch<any>(`${this.apiUrl}Logical/${id}`, {});
  }
}
