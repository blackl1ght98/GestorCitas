export interface IUsuarioUpdate {
  Email: string;
  Password: string;
  NombreCompleto: string;
  FechaNacimiento: Date;
  Telefono: string;
  Direccion: string;
  FechaRegistro: Date;
  cita: string[];
  insulina: boolean;
  IdUsuarioNavigation?: any[];
}
