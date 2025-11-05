import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { CreatePlotTransferHistoryRoutingModule } from './create-plot-transfer-history-routing.module';
import { CreatePlotTransferHistoryComponent } from './create-plot-transfer-history.component';
import { PageModule } from '@abp/ng.components/page';
import { SharedModule } from 'src/app/shared/shared.module';


@NgModule({
  declarations: [
    CreatePlotTransferHistoryComponent
  ],
  imports: [
    CommonModule,
    CreatePlotTransferHistoryRoutingModule,
    PageModule,
    SharedModule
  ]
})
export class CreatePlotTransferHistoryModule { }
