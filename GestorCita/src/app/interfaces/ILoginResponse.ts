export interface ILoginResponse {
  Id: number;
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
  Id: number;
  NewPass: string;
}
export interface IRecoverPass {
  Token: string;
  NewPass: string;
}

export interface IUserLogout {
  email: string;
}
