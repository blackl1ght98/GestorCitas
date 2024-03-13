export interface ILoginResponse {
  id: number;
  token: string;
  Rol: string;
  NombreCompleto: string;
  Email: string;
  Direccion: string;
  Telefono: string;
  FechaNacimiento: Date;
  FechaRegistro: Date;
}
export interface ILogin {
  Email: string;
  Password: string;
}
export interface IChangePass {
  id: number;
  newPass: string;
}
export interface IRecoverPass {
  token: string;
  newPass: string;
}

export interface IUserLogout {
  email: string;
}
