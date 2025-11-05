import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { PlotInfoRoutingModule } from './plot-info-routing.module';
import { PlotInfoComponent } from './plot-info.component';
import { PageModule } from '@abp/ng.components/page';
import { SharedModule } from '../shared/shared.module';


@NgModule({
  declarations: [
    PlotInfoComponent
  ],
  imports: [
    CommonModule,
    PlotInfoRoutingModule,
    PageModule,
    SharedModule
  ]
})
export class PlotInfoModule { }
