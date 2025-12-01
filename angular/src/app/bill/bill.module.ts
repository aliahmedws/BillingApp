import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { BillRoutingModule } from './bill-routing.module';
import { BillComponent } from './bill.component';
import { SharedModule } from '../shared/shared.module';
import { NzTabsModule } from 'ng-zorro-antd/tabs';
import { PageModule } from '@abp/ng.components/page';


@NgModule({
  declarations: [
    BillComponent
  ],
  imports: [
    CommonModule,
    BillRoutingModule,
    SharedModule,
    NzTabsModule,
    PageModule
  ]
})
export class BillModule { }
