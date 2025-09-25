import { Component } from '@angular/core';

import { NzButtonModule } from 'ng-zorro-antd/button';

import { AuthService } from '@auth0/auth0-angular';
import { Router } from '@angular/router';

@Component({
  selector: 'app-login',
  imports: [NzButtonModule],
  templateUrl: './login.component.html',
  styleUrl: './login.component.css',
})
export class LoginComponent {
  constructor(public auth: AuthService, private router: Router) {}

  ngOnInit() {
    this.auth.isAuthenticated$.subscribe((isAuth) => {
      if (isAuth) {
        this.router.navigate(['/reservation']);
      }
    });
  }
}
