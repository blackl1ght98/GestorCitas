import { CommonModule } from '@angular/common';
import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { ICita } from '../../interfaces/ICita';
import { CitaService } from '../../services/cita.service';

@Component({
  selector: 'app-tabla-cita',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './tabla-cita.component.html',
  styleUrl: './tabla-cita.component.css',
})
export class TablaCitaComponent implements OnInit {
  @Input() cita: ICita[] = [];
  citas: ICita[] = [];
  constructor(private citaService: CitaService) {}
  mostrarMensajeActualizar = false;
  ngOnInit(): void {
    this.citaService.citas$.subscribe((citas) => {
      this.citas = citas;
      // No necesitas asignar citas a this.cita aquí, ya que lo haces en el template
    });
    this.ObtenerCita();
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
          this.cita = citas;
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
  reloadPage() {
    this.mostrarMensajeActualizar = true;
    location.reload();
  }
  eliminarCita(idCita: number) {
    this.citaService.deleteCita(idCita).subscribe({
      next: (response) => {
        // Imprimir la respuesta del servidor
        console.log(response);
        // Actualizar la lista de citas
        this.ObtenerCita();
      },
      error: (error) => {
        console.error('Error al eliminar la cita:', error);
      },
    });
  }
}
