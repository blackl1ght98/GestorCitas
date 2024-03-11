export interface ICita {
  Id?: number;
  idUsuario?: number;
  idUsuarioNavigation?: null;
  fechaYhora: Date;
  motivoCita: string;
  ubicacionCita: string; // Agrega este campo
  duracionEstimada: string;
  nombreDelProfesional: string;
  notasAdicionales: string;
  estadoCita: string;
}
