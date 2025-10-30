import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { SocietychargeRoutingModule } from './societycharge-routing.module';
import { SocietychargeComponent } from './societycharge.component';
import { SharedModule } from '../shared/shared.module';
import { PageModule } from "@abp/ng.components/page";


@NgModule({
  declarations: [
    SocietychargeComponent
  ],
  imports: [
    CommonModule,
    SocietychargeRoutingModule,
    SharedModule,
    PageModule
]
})
export class SocietychargeModule { }
