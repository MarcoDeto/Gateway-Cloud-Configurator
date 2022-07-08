import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { HomeGuard } from '../Shared/Guards/home.guard';
import { GatewayDetailsComponent } from './Components/gateway-detail/gateway-detail.component';
import { HomeComponent } from './Components/home/home.component';
import { InterfaceGroupsComponent } from './Components/interface-groups/interface-groups.component';
import { InterfaceDetailComponent } from './Components/interface-detail/interface-detail.component';

const coreRoutes: Routes = [
  { path: 'home', component: HomeComponent, canActivate: [HomeGuard] } ,
  { path: 'gateway/:gatewayId', component: GatewayDetailsComponent, canActivate: [HomeGuard] },
  { path: 'gateway/interfacegroups/:gatewayId', component: InterfaceGroupsComponent, canActivate: [HomeGuard] },
  { path: 'interface-detail/:gatewayId/:groupId/:interfaceId', component: InterfaceDetailComponent, canActivate: [HomeGuard] }
];

@NgModule({
  imports: [RouterModule.forChild(coreRoutes)],
  exports: [RouterModule]
})
export class CoreRoutingModule { }

