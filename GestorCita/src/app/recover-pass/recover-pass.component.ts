import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { RecordarPassService } from '../services/recordar-pass.service';
import { Router, ActivatedRoute } from '@angular/router';
import { IRecoverPass } from '../interfaces/ILoginResponse';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-recover-pass',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './recover-pass.component.html',
  styleUrl: './recover-pass.component.css',
})
export class RecoverPassComponent {
  token: string = '';
  newPass: string = '';
  newPass2: string = '';
  constructor(
    private recordarPassService: RecordarPassService,
    private router: Router,
    private activatedRoute: ActivatedRoute
  ) {
    this.activatedRoute.params.subscribe((params) => {
      this.token = params['token'];
      console.log(this.token);
    });
  }
  recuperarPass() {
    const datoLogin: IRecoverPass = {
      token: this.token,
      newPass: this.newPass,
    };
    console.log(datoLogin);

    this.recordarPassService.recordarPass(datoLogin).subscribe({
      next: (res) => {
        this.router.navigate(['/login']);
        console.log(res);
      },
      error: (err) => {
        console.error('esto es error al recuperar pass' + err);
      },
    });
  }
}
