import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
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
export class CitaComponent {
  datocita: ICita = {
    FechaYHora: new Date(),
    MotivoCita: '',
    DuracionEstimada: '',
    UbicacionCita: '',
    NombreDelProfesional: '',
    NotasAdicionales: '',
    EstadoCita: '',
  };
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
}
