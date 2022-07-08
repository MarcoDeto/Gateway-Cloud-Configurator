import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HomeComponent } from './Components/home/home.component';
import { CoreRoutingModule } from './core-routing.module';
import { MaterialModule } from '../material.module';
import { ReactiveFormsModule } from '@angular/forms';
import { SharedModule } from '../Shared/shared.module';
import { InterfaceGroupsComponent } from './Components/interface-groups/interface-groups.component';
import { InterfaceGroupDatailComponent } from './Components/interface-groups/interface-groups-datail/interface-group-datail.component';
import { TypeEntityComponent } from './Components/type-entity/type-entity.component';
import { CardGatewayComponent } from './Components/home/card-gateway/card-gateway.component';
import { InterfaceDetailComponent } from './Components/interface-detail/interface-detail.component';
import { ParametersComponent } from './Components/interface-detail/parameters/parameters.component';
import { ChannelsComponent } from './Components/interface-detail/channels/channels.component';
import { VirtualChannelsComponent } from './Components/interface-detail/virtual-channels/virtual-channels.component';
import { VariablesComponent } from './Components/interface-detail/channels/variables/variables.component';
import { VirtualChannelDatailsComponent } from './Components/interface-detail/virtual-channels/virtual-channel-datails/virtual-channel-datails.component';
import { ChannelAssociateComponent } from './Components/interface-detail/virtual-channels/virtual-channel-datails/channel-associate/channel-associate.component';
import { RulesComponent } from './Components/interface-detail/virtual-channels/virtual-channel-datails/rules/rules.component';
import { GatewayDetailsComponent } from './Components/gateway-detail/gateway-detail.component';

@NgModule({
  declarations: [
    HomeComponent,
    InterfaceGroupsComponent,
    InterfaceGroupDatailComponent,
    TypeEntityComponent,
    CardGatewayComponent,
    InterfaceDetailComponent,
    ParametersComponent,
    ChannelsComponent,
    VirtualChannelsComponent,
    VariablesComponent,
    VirtualChannelDatailsComponent,
    ChannelAssociateComponent,
    RulesComponent,
    GatewayDetailsComponent,
  ],
  imports: [
    SharedModule,
    CommonModule,
    CoreRoutingModule,
    MaterialModule,
    ReactiveFormsModule
  ],
  exports: [
    HomeComponent,
    InterfaceGroupsComponent,
    GatewayDetailsComponent
  ]
})
export class CoreModule { }
