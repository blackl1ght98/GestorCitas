import { Routes } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { CitaComponent } from './formularios/cita/cita.component';
import { LoginComponent } from './login/login.component';
import { RegisterComponent } from './register/register.component';
import { MisDatosComponent } from './mis-datos/mis-datos.component';

export const routes: Routes = [
  { path: '', redirectTo: 'home', pathMatch: 'full' },
  { path: 'home', component: HomeComponent },
  { path: 'cita', component: CitaComponent },
  { path: 'login', component: LoginComponent },
  { path: 'registro', component: RegisterComponent },
  { path: 'mis-datos', component: MisDatosComponent },
];
