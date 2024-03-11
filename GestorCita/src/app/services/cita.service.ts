import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { ICita } from '../interfaces/ICita';
import { environment } from '../environments/environment';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class CitaService {
  private API_URL = environment.apiUrl;

  constructor(private http: HttpClient) {}
  postCita(cita: ICita): Observable<ICita> {
    const token = localStorage.getItem('token');
    console.log('esto es el token: ' + token);
    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
      Authorization: `Bearer ${token}`,
    });

    return this.http
      .post<ICita>(`${this.API_URL}/Cita`, cita, { headers: headers })
      .pipe();
  }
  deleteCita(idCita: number): Observable<any> {
    const token = localStorage.getItem('token');
    console.log('esto es el token: ' + token);
    // Verifica si el token está presente en el localStorage

    // Configura el encabezado de autorización con el token
    const headers = new HttpHeaders({
      Authorization: `Bearer ${token}`,
    });
    const options = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
      }),
      body: {
        id: idCita,
      },
    };

    return this.http.delete<any>(`${this.API_URL}/Cita/eliminarcita`, {
      ...options,
      responseType: 'text' as 'json',
      headers: headers,
    });
  }
}
