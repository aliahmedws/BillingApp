import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { IescochargeRoutingModule } from './iescocharge-routing.module';
import { IescochargeComponent } from './iescocharge.component';
import { SharedModule } from '../shared/shared.module';
import { PageModule } from "@abp/ng.components/page";


@NgModule({
  declarations: [
    IescochargeComponent
  ],
  imports: [
    CommonModule,
    IescochargeRoutingModule,
    PageModule,
    SharedModule
]
})
export class IescochargeModule { }
