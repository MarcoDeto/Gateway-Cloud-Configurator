import { NgModule } from '@angular/core';
import { RouterModule, Routes, PreloadAllModules } from '@angular/router';
import { LoginComponent } from './Authentication/login/login.component';
import { HomeComponent } from './Core/Components/home/home.component';
import { ErrorComponent } from './Shared/Components/error/error.component';
import { ForbiddenComponent } from './Shared/Components/Forbidden/forbidden.component';
import { HomeGuard } from './Shared/Guards/home.guard';

const routes: Routes = [
  { path: 'login', component: LoginComponent},
  { path: '',   redirectTo: 'home', pathMatch: 'full' },
  { path: 'home', component: HomeComponent, canActivate: [HomeGuard], loadChildren: () => import('./Core/core.module').then(mod => mod.CoreModule) },
  { path: 'forbidden', component: ForbiddenComponent },
  { path: 'errore', component: ErrorComponent }
];

@NgModule({
  imports: [RouterModule.forRoot(routes, { useHash: false, preloadingStrategy: PreloadAllModules })],
  exports: [RouterModule]
})
export class AppRoutingModule { }