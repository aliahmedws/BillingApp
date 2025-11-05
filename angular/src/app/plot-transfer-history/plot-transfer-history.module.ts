import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { PlotTransferHistoryRoutingModule } from './plot-transfer-history-routing.module';
import { PlotTransferHistoryComponent } from './plot-transfer-history.component';
import { SharedModule } from '../shared/shared.module';
import { PageModule } from '@abp/ng.components/page';


@NgModule({
  declarations: [
    PlotTransferHistoryComponent
  ],
  imports: [
    CommonModule,
    PlotTransferHistoryRoutingModule,
    SharedModule,
    PageModule
  ]
})
export class PlotTransferHistoryModule { }
