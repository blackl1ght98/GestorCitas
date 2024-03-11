import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormsModule, NgForm } from '@angular/forms';
import { ICita } from '../../interfaces/ICita';
import { CitaService } from '../../services/cita.service';
import { Router } from '@angular/router';
import { HttpClientModule } from '@angular/common/http';
import { TablaCitaComponent } from '../tabla-cita/tabla-cita.component';

@Component({
  selector: 'app-cita',
  standalone: true,
  imports: [CommonModule, FormsModule, HttpClientModule, TablaCitaComponent],
  templateUrl: './cita.component.html',
  styleUrl: './cita.component.css',
})
export class CitaComponent implements OnInit {
  datocita: ICita = {
    fechaYhora: new Date(),
    motivoCita: '',
    duracionEstimada: '',
    ubicacionCita: '',
    nombreDelProfesional: '',
    notasAdicionales: '',
    estadoCita: '',
  };
  ngOnInit(): void {
    this.ObtenerCita();
  }
  misCitas: ICita[] = [];
  constructor(private router: Router, private citaService: CitaService) {}
  onSubmit(form: NgForm) {
    this.citaService.postCita(this.datocita).subscribe({
      next: (res) => {
        console.log(res);
        this.router.navigate(['/mis-datos']);
      },
      error: (err) => {
        console.log(err);
      },
    });

    // Formulario inválido, mostrar mensajes de validación si es necesario
  }
  ObtenerCita() {
    const idStr = localStorage.getItem('id'); // Obtener la ID como cadena desde el localStorage
    const idNum = parseInt(idStr!, 10); // Convertir la cadena a número usando parseInt()

    // Verificar si idNum es un número válido antes de usarlo
    if (!isNaN(idNum)) {
      // El valor almacenado en id es un número válido
      console.log('ID convertida a número:', idNum);

      // Aquí puedes usar idNum para obtener las citas
      this.citaService.getCitaPorId(idNum).subscribe({
        next: (citas) => {
          // Manejar las citas obtenidas aquí
          console.log(citas);
          // Asignar las citas al componente
          this.misCitas = citas;
        },
        error: (error) => {
          console.error('Error al obtener citas:', error);
        },
      });
    } else {
      console.error(
        'El valor de la ID almacenado en el localStorage no es un número válido'
      );
    }
  }
}
