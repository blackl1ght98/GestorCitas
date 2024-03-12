import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { ICita } from '../interfaces/ICita';
import { environment } from '../environments/environment';
import { BehaviorSubject, Observable, tap } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class CitaService {
  private API_URL = environment.apiUrl;
  private citasSource = new BehaviorSubject<ICita[]>([]);
  citas$ = this.citasSource.asObservable();
  constructor(private http: HttpClient) {}
  // getCitaPorId(userId: number): Observable<any> {
  //   const token = localStorage.getItem('token');
  //   console.log('esto es el token: ' + token);
  //   const headers = new HttpHeaders({
  //     'Content-Type': 'application/json',
  //     Authorization: `Bearer ${token}`,
  //   });
  //   return this.http.get<any>(
  //     `${this.API_URL}/Cita/getcitaporidusuario/${userId}`,
  //     { headers: headers }
  //   );
  // }
  // postCita(cita: ICita): Observable<ICita> {
  //   const token = localStorage.getItem('token');
  //   console.log('esto es el token: ' + token);
  //   const headers = new HttpHeaders({
  //     'Content-Type': 'application/json',
  //     Authorization: `Bearer ${token}`,
  //   });

  //   return this.http
  //     .post<ICita>(`${this.API_URL}/Cita`, cita, { headers: headers })
  //     .pipe();
  // }
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
  getCitaPorId(userId: number): Observable<any> {
    const token = localStorage.getItem('token');
    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
      Authorization: `Bearer ${token}`,
    });
    return this.http
      .get<any>(`${this.API_URL}/Cita/getcitaporidusuario/${userId}`, {
        headers: headers,
      })
      .pipe(
        tap((citas: ICita[]) => {
          this.citasSource.next(citas);
        })
      );
  }

  // postCita(cita: ICita): Observable<ICita> {
  //   const token = localStorage.getItem('token');
  //   const headers = new HttpHeaders({
  //     'Content-Type': 'application/json',
  //     Authorization: `Bearer ${token}`,
  //   });

  //   return this.http
  //     .post<ICita>(`${this.API_URL}/Cita`, cita, { headers: headers })
  //     .pipe(
  //       tap((nuevaCita: ICita) => {
  //         const citasActualizadas = [...this.citasSource.value, nuevaCita];
  //         this.citasSource.next(citasActualizadas);
  //       })
  //     );
  // }
  postCita(cita: ICita): Observable<ICita> {
    const token = localStorage.getItem('token');
    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
      Authorization: `Bearer ${token}`,
    });

    // Agregar la nueva cita a la lista existente antes de enviarla al servidor
    const citasActualizadas = [...this.citasSource.value, cita];
    this.citasSource.next(citasActualizadas);

    return this.http
      .post<ICita>(`${this.API_URL}/Cita`, cita, { headers: headers })
      .pipe(
        tap((nuevaCita: ICita) => {
          // No necesitas actualizar el BehaviorSubject aquí, ya lo has hecho antes de la llamada al servidor
        })
      );
  }
}
