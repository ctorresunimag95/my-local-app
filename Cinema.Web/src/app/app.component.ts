import { Component } from '@angular/core';
import { RouterOutlet, Router, RouterLink } from '@angular/router';
import { NzBreadCrumbModule } from 'ng-zorro-antd/breadcrumb';
import { NzIconModule } from 'ng-zorro-antd/icon';
import { NzLayoutModule } from 'ng-zorro-antd/layout';
import { NzMenuModule } from 'ng-zorro-antd/menu';
import { GlobalAlertComponent } from './shared/components/alert/global.alert.component';
import { AuthService } from '@auth0/auth0-angular';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, 
    NzBreadCrumbModule, 
    NzIconModule, 
    NzMenuModule, 
    NzLayoutModule,
    GlobalAlertComponent,
    RouterLink
  ],
  standalone: true,
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent {
  title = 'Cinema.Web';

  constructor(public router: Router, private auth: AuthService) { }

  logout() {
    this.auth.logout();
  }
}
