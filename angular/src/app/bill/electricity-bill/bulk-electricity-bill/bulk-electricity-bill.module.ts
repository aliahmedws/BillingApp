import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { BulkElectricityBillRoutingModule } from './bulk-electricity-bill-routing.module';
import { BulkElectricityBillComponent } from './bulk-electricity-bill.component';
import { SharedModule } from 'src/app/shared/shared.module';
import { PageModule } from '@abp/ng.components/page';


@NgModule({
  declarations: [
    BulkElectricityBillComponent
  ],
  imports: [
    CommonModule,
    BulkElectricityBillRoutingModule,
    SharedModule,
    PageModule
  ]
})
export class BulkElectricityBillModule { }
