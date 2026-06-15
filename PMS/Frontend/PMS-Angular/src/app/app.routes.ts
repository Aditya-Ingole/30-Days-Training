import { Routes } from '@angular/router';
import { Dashboard } from './features/dashboard/dashboard/dashboard';
import { ProductList } from './features/products/product-list/product-list';
import { LoginComponent } from './features/auth/login/login';
import { RegisterComponent } from './features/auth/register/register';
import { authGuard } from './core/guards/auth-guard';



export const routes: Routes = [
    {
        path: '',
        redirectTo: 'login',
        pathMatch: 'full'
    },

    {
        path: 'login',
        component: LoginComponent
    },

    {
        path: 'dashboard',
        component: Dashboard,
        canActivate: [authGuard]
    },

    {
        path: 'products',
        component: ProductList,
        canActivate: [authGuard]
    },
    
    {
        path: 'register',
        component: RegisterComponent
    },

    {
        path: '**',
        redirectTo: 'login'
    }, 

    
];
