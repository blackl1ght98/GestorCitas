import { CommonModule } from '@angular/common';
import { Component, Input } from '@angular/core';
import { ICita } from '../../interfaces/ICita';
import { CitaService } from '../../services/cita.service';

@Component({
  selector: 'app-tabla-cita',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './tabla-cita.component.html',
  styleUrl: './tabla-cita.component.css',
})
export class TablaCitaComponent {
  @Input() citas: ICita[] = [];
  constructor(private citaService: CitaService) {}
  eliminarCita(cita: number) {
    this.citaService.deleteCita(cita);
  }
}
