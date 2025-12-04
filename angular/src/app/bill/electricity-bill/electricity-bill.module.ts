import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { ElectricityBillRoutingModule } from './electricity-bill-routing.module';
import { ElectricityBillComponent } from './electricity-bill.component';
import { SharedModule } from 'src/app/shared/shared.module';
import { PageModule } from '@abp/ng.components/page';


@NgModule({
  declarations: [
    ElectricityBillComponent
  ],
  imports: [
    CommonModule,
    ElectricityBillRoutingModule,
    SharedModule,
    PageModule
  ]
})
export class ElectricityBillModule { }
