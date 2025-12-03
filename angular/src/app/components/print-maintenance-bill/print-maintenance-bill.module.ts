import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { PrintMaintenanceBillRoutingModule } from './print-maintenance-bill-routing.module';
import { PrintMaintenanceBillComponent } from './print-maintenance-bill.component';
import { SharedModule } from 'src/app/shared/shared.module';


@NgModule({
  declarations: [
    PrintMaintenanceBillComponent
  ],
  imports: [
    CommonModule,
    PrintMaintenanceBillRoutingModule,
    SharedModule
  ]
})
export class PrintMaintenanceBillModule { }
