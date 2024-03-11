import { Component } from '@angular/core';
import { ILogin } from '../interfaces/ILoginResponse';
import { Router } from '@angular/router';
import { AuthService } from '../services/auth.service';
import { FormsModule, NgForm } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { CitaService } from '../services/cita.service';
import { TablaCitaComponent } from '../formularios/tabla-cita/tabla-cita.component';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './login.component.html',
  styleUrl: './login.component.css',
})
export class LoginComponent {
  datoLogin: ILogin = {
    Email: '',
    Password: '',
  };
  constructor(
    private router: Router,
    private authService: AuthService,
    private citaService: CitaService
  ) {}
  citas: any[] = [];
  onSubmit(form: NgForm) {
    if (form.valid) {
      this.authService.loginUser(this.datoLogin).subscribe({
        next: (res) => {
          const id = res.id;
          this.citaService.getCitaPorId(id).subscribe({
            next: (citas) => {
              // Maneja las citas aquí según sea necesario
              console.log(citas);
              this.citas = citas;
            },
            error: (error) => {
              console.error('Error al obtener citas:', error);
            },
          });
          console.log(res);
          this.router.navigate(['/mis-datos']);
        },
        error: (err) => {
          console.log(err);
        },
      });
    } else {
      // Formulario inválido, mostrar mensajes de validación si es necesario
    }
  }
}
