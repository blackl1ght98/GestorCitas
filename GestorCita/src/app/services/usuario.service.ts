import { Injectable } from '@angular/core';
import { environment } from '../environments/environment';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { IRegistro } from '../interfaces/IRegistro';
import { Observable } from 'rxjs';
import { IUsuarioUpdate } from '../interfaces/IUsuarioUpdate';

@Injectable({
  providedIn: 'root',
})
export class UsuarioService {
  private API_URL = environment.apiUrl;
  constructor(private http: HttpClient) {}
  //Obtener todos los datos del usuario + sus citas
  getUserById(userId: number): Observable<IUsuarioUpdate> {
    const token = localStorage.getItem('token');
    console.log('esto es el token: ' + token);
    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
      Authorization: `Bearer ${token}`,
    });
    return this.http.get<IUsuarioUpdate>(
      `${this.API_URL}/Users/usuarioPorId/${userId}`,
      { headers: headers }
    );
  }
  actualizarUsuario(usuario: IUsuarioUpdate): Observable<IUsuarioUpdate> {
    const token = localStorage.getItem('token');
    console.log('esto es el token: ' + token);
    // Verifica si el token está presente en el localStorage

    // Configura el encabezado de autorización con el token
    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
      Authorization: `Bearer ${token}`,
    });

    // Convierte la propiedad medicacion a un array de strings si es necesario
    // if (typeof usuario.cita === 'string') {
    //   usuario.cita = [usuario.cita];
    // }

    return this.http
      .patch<IUsuarioUpdate>(
        `${this.API_URL}/Users/cambiardatosusuario`,
        usuario,
        { headers: headers }
      )
      .pipe();
  }

  cambiarPass(data: { id: number; NewPass: string }): Observable<string> {
    return this.http.put(
      `${this.API_URL}/ChangePasswordC/changePassword`,
      data,
      { responseType: 'text' }
    );
  }
}
