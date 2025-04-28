import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../environments/environment.development';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class PersonService {

  private baseUrl = `${environment.apiURL}api/Person`;

  constructor(private http: HttpClient) {}

  getAll(): Observable<any[]> {
    return this.http.get<any[]>(this.baseUrl);
  }

  getById(id: number): Observable<any> {
    return this.http.get<any>(`${this.baseUrl}/${id}`);
  }

  getAvailable(): Observable<any[]> {
    return this.http.get<any[]>(`${this.baseUrl}/available`);
  }

  create(person: any): Observable<any> {
    return this.http.post<any>(`${this.baseUrl}`, person);
  }

  update(person: any): Observable<any> {
    return this.http.put<any>(`${this.baseUrl}/update`, person);
  }

  delete(id: number): Observable<any> {
    return this.http.delete<any>(`${this.baseUrl}/${id}`);
  }

  logicalDelete(id: number): Observable<any> {
    return this.http.patch<any>(`${this.baseUrl}/Logical/${id}`, {});
  }

}
