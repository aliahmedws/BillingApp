import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { PlotSizeRoutingModule } from './plot-size-routing.module';
import { PlotSizeComponent } from './plot-size.component';
import { SharedModule } from '../shared/shared.module';
import { PageModule } from '@abp/ng.components/page';


@NgModule({
  declarations: [
    PlotSizeComponent
  ],
  imports: [
    CommonModule,
    PlotSizeRoutingModule,
    SharedModule,
    PageModule
  ]
})
export class PlotSizeModule { }
