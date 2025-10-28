import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { PlotTypeRoutingModule } from './plot-type-routing.module';
import { PlotTypeComponent } from './plot-type.component';
import { SharedModule } from '../shared/shared.module';
import { PageModule } from '@abp/ng.components/page';


@NgModule({
  declarations: [
    PlotTypeComponent
  ],
  imports: [
    CommonModule,
    PlotTypeRoutingModule,
    SharedModule,
    PageModule
  ]
})
export class PlotTypeModule { }
