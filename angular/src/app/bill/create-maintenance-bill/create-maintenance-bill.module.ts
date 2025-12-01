import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { CreateMaintenanceBillRoutingModule } from './create-maintenance-bill-routing.module';
import { CreateMaintenanceBillComponent } from './create-maintenance-bill.component';
import { SharedModule } from 'src/app/shared/shared.module';
import { PageModule } from '@abp/ng.components/page';


@NgModule({
  declarations: [
    CreateMaintenanceBillComponent
  ],
  imports: [
    CommonModule,
    CreateMaintenanceBillRoutingModule,
    SharedModule,
    PageModule
  ]
})
export class CreateMaintenanceBillModule { }
