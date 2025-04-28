import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../environments/environment.development';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class FormModuleService {

  private apiUrl = `${environment.apiURL}api/FormModule`;

  constructor(private http: HttpClient) { }

  getAll(): Observable<any[]> {
    return this.http.get<any[]>(this.apiUrl);
  }

  getById(id: number): Observable<any> {
    return this.http.get<any>(`${this.apiUrl}/${id}`);
  }

  create(formModule: any): Observable<any> {
    return this.http.post<any>(this.apiUrl, formModule);
  }

  update(formModule: any): Observable<any> {
    return this.http.put<any>(`${this.apiUrl}/${formModule.id}`, formModule);
  }

  delete(id: number): Observable<any> {
    return this.http.delete<any>(`${this.apiUrl}/${id}`);
  }

  logicalDelete(id: number): Observable<any> {
    return this.http.patch<any>(`${this.apiUrl}/Logical/${id}`, {});
  }
}
