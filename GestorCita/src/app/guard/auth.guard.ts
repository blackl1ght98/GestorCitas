import {
  ActivatedRouteSnapshot,
  RouterStateSnapshot,
  UrlTree,
} from '@angular/router'; // Importa los tipos necesarios para ActivatedRouteSnapshot, RouterStateSnapshot y UrlTree
import { Router } from '@angular/router'; // Importa el enrutador de Angular
import { map } from 'rxjs/operators'; // Importa el operador map de RxJS
import { Injectable } from '@angular/core'; // Importa el decorador Injectable de Angular
import { Observable } from 'rxjs'; // Importa Observable de RxJS
import { AuthService } from '../services/auth.service';

@Injectable({
  providedIn: 'root', // Indica que este servicio estará disponible en el módulo raíz
})
export class AuthGuard {
  // Define una clase para el guardia
  constructor(
    private authService: AuthService, // Inyecta el servicio de autenticación
    private router: Router // Inyecta el enrutador de Angular
  ) {}

  canActivate(
    route: ActivatedRouteSnapshot, // Proporciona información sobre la ruta activada actualmente
    state: RouterStateSnapshot // Proporciona información sobre el estado de enrutamiento actual
  ): Observable<boolean | UrlTree> {
    // Devuelve un observable de tipo booleano o UrlTree
    return this.authService.user.pipe(
      // Accede al observable user del servicio de autenticación
      map((user) => {
        // Utiliza el operador map para transformar el valor del observable
        if (user && user.token) {
          // Verifica si el usuario está autenticado y tiene un token válido
          return true; // Si el usuario está autenticado y tiene un token válido, permite la navegación
        }

        const returnUrl = state.url; // Obtiene la URL de retorno del estado de enrutamiento actual
        this.router.navigate(['/login'], { queryParams: { returnUrl } }); // Redirige al usuario a la página de inicio de sesión con la URL de retorno como parámetro de consulta
        return false; // Devuelve falso para bloquear la navegación
      })
    );
  }
}
