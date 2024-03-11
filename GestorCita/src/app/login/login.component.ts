import { Component } from '@angular/core';
import { ILogin } from '../interfaces/ILoginResponse';
import { Router } from '@angular/router';
import { AuthService } from '../services/auth.service';
import { FormsModule, NgForm } from '@angular/forms';
import { CommonModule } from '@angular/common';

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
  constructor(private router: Router, private authService: AuthService) {}
  onSubmit(form: NgForm) {
    if (form.valid) {
      this.authService.loginUser(this.datoLogin).subscribe({
        next: (res) => {
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
