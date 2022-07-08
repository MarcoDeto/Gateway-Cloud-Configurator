import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { LoginComponent } from './login/login.component';
import { HomeGuard } from '../Shared/Guards/home.guard';
import { SharedModule } from '../Shared/shared.module';
import { HTTP_INTERCEPTORS } from '@angular/common/http';
import { AuthInterceptor } from '../Shared/Interceptors/auth.interceptor';
import { MaterialModule } from '../material.module';



@NgModule({
  declarations: [LoginComponent],
  imports: [
    MaterialModule,
    SharedModule,
    CommonModule
  ],
  exports: [
    LoginComponent
  ],
  providers: [
    HomeGuard,
    {
      provide: HTTP_INTERCEPTORS,
      useClass: AuthInterceptor,
      multi: true
    }
  ]
})
export class AuthenticationModule { }