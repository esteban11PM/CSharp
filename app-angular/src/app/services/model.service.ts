import { inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment.development';

@Injectable({
  providedIn: 'root'
})
export class ModelService {
  private http = inject(HttpClient);
  private baseUrl: string = `${environment.apiURL}api/Module/`;

  getAll(): Observable<any[]> {
    return this.http.get<any[]>(`${this.baseUrl}`);
  }
  getById(id: number): Observable<any> {
    return this.http.get(`${this.baseUrl}${id}`);
  }
  create(model: any): Observable<any> {
    return this.http.post(`${this.baseUrl}`, model);
  }
  update(model: any): Observable<any> {
    return this.http.put(`${this.baseUrl}update`, model);
  }
  delete(id: number): Observable<any> {
    return this.http.delete(`${this.baseUrl}${id}`);
  }
  deletelogical(id: number): Observable<any> {
    return this.http.delete(`${this.baseUrl}Delete/${id}`);
  }
  logicalDelete(id: number) {
    return this.http.patch(`${this.baseUrl}logical/${id}`, null);
  }
}
