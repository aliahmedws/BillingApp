import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { CreateElectricityBillRoutingModule } from './create-electricity-bill-routing.module';
import { CreateElectricityBillComponent } from './create-electricity-bill.component';
import { SharedModule } from 'src/app/shared/shared.module';
import { PageModule } from '@abp/ng.components/page';


@NgModule({
  declarations: [
    CreateElectricityBillComponent
  ],
  imports: [
    CommonModule,
    CreateElectricityBillRoutingModule,
    SharedModule,
    PageModule
  ]
})
export class CreateElectricityBillModule { }
