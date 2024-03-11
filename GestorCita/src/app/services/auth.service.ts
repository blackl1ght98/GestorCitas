import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../environments/environment.development';
import { ILogin, ILoginResponse } from '../interfaces/ILoginResponse';
import { BehaviorSubject, Observable, map } from 'rxjs';
import { IRegistro } from '../interfaces/IRegistro';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private API_URL = environment.apiUrl;
  private currentUserSubject: BehaviorSubject<ILoginResponse | null>;
  //Y con esta obtenemos el valor del usuario que se ha logueado ( es decir, sus datos)
  public user: Observable<ILoginResponse | null>;

  constructor(private http: HttpClient) {
    //Esta linea es para guardar el usuario que se ha logueado en el localstorage del navegador
    this.currentUserSubject = new BehaviorSubject<ILoginResponse | null>(
      JSON.parse(localStorage.getItem('user') || '{}')
    );
    // Un observable es un objeto que emite notificaciones cuando cambia el valor de una propiedad
    //Con esto podemos obtener el usuario que se ha logueado
    this.user = this.currentUserSubject.asObservable();
  }
  //Metodo para acceder al valor del usuario que se ha logueado
  public get userValue(): ILoginResponse | null {
    return this.currentUserSubject.value;
  }
  registerUser(datoRegistro: IRegistro): Observable<IRegistro> {
    return this.http.post<IRegistro>(
      `${this.API_URL}/Users/registro`,
      datoRegistro
    );
  }
  loginUser(datoLogin: ILogin): Observable<ILoginResponse> {
    return this.http
      .post<ILoginResponse>(`${this.API_URL}/Users/login`, datoLogin)
      .pipe(
        map((response: ILoginResponse) => {
          localStorage.setItem('user', JSON.stringify(response)); // Guardar el objeto completo en el localStorage

          const token = response.token; // Extraer el token del objeto de respuesta
          console.log('esto es el token: ' + token);
          localStorage.setItem('token', token); // Guardar el token en el localStorage
          this.currentUserSubject.next(response); // Emitir el usuario completo (o solo el token si lo necesitas)
          return response; // Devolver la respuesta completa (opcional)
        })
      );
  }
  //Metodo para cerrar sesion
  logoutUser(): void {
    //Eliminamos el usuario del localstorage
    localStorage.removeItem('user');
    //Con next podemos emitir un nuevo valor, en este caso null, por que el usuario se ha deslogueado
    this.currentUserSubject.next(null);
  }
}
