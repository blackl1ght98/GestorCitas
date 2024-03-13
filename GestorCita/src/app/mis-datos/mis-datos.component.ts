import { Component, OnInit } from '@angular/core';
import { IUsuarioUpdate } from '../interfaces/IUsuarioUpdate';
import { UsuarioService } from '../services/usuario.service';
import { CommonModule } from '@angular/common';
import { FormsModule, NgForm } from '@angular/forms';
import { IChangePass } from '../interfaces/ILoginResponse';

@Component({
  selector: 'app-mis-datos',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './mis-datos.component.html',
  styleUrl: './mis-datos.component.css',
})
export class MisDatosComponent implements OnInit {
  actualizacionExitosa = false;
  datoUsuario: IUsuarioUpdate = {
    email: '',
    nombreCompleto: '',
    fechaNacimiento: new Date(),
    telefono: '',
    direccion: '',
  };
  ngOnInit(): void {
    const idStr = localStorage.getItem('id'); // Obtener la ID como cadena desde el localStorage
    const idNum = parseInt(idStr!, 10); // Convertir la cadena a número usando parseInt()

    // Verificar si idNum es un número válido antes de usarlo
    if (!isNaN(idNum)) {
      this.usuarioService.getUserById(idNum).subscribe((usuario) => {
        console.log('esto es el usuario', usuario);
        this.datoUsuario = usuario;
      });
    }
  }
  constructor(private usuarioService: UsuarioService) {}
  onSubmit(form: NgForm) {
    this.usuarioService
      .actualizarUsuario(this.datoUsuario)
      .subscribe((usuarioActualizado) => {
        this.actualizacionExitosa = true;
        console.log('Usuario actualizado', usuarioActualizado);
        // Aquí puedes manejar la respuesta del servidor, por ejemplo, mostrando un mensaje de éxito.
      });
  }

  cambioPass: IChangePass = {
    id: 0,
    newPass: '',
  };
  cambioPassService() {
    const idStr = localStorage.getItem('id'); // Obtener la ID como cadena desde el localStorage
    const idNum = parseInt(idStr!, 10); // Convertir la cadena a número usando parseInt()

    // Verificar si idNum es un número válido antes de usarlo
    if (!isNaN(idNum)) {
      this.cambioPass.id = idNum;

      this.usuarioService.cambiarPass(this.cambioPass).subscribe({
        next: (res) => {
          console.log(res);
        },
        error: (err) => {
          console.log(err);
        },
      });
    }
  }
}
