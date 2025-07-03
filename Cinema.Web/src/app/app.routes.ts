import { Routes } from '@angular/router';
import { HomeComponent } from './home/home.component';

export const routes: Routes = [
    {
        path: 'management',
        loadChildren: () => import('./management/management.routes').then(m => m.MANAGEMENT_ROUTES)
    },
    {
        path: 'reservation',
        component: HomeComponent
    },
    {
        path: '',
        component: HomeComponent,
        pathMatch: 'full'
    }
];
