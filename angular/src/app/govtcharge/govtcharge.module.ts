import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { GovtchargeRoutingModule } from './govtcharge-routing.module';
import { GovtchargeComponent } from './govtcharge.component';
import { PageModule } from "@abp/ng.components/page";
import { SharedModule } from '../shared/shared.module';


@NgModule({
  declarations: [
    GovtchargeComponent
  ],
  imports: [
    CommonModule,
    GovtchargeRoutingModule,
    PageModule,
    SharedModule
]
})
export class GovtchargeModule { }
