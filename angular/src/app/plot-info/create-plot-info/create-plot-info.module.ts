import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { CreatePlotInfoRoutingModule } from './create-plot-info-routing.module';
import { CreatePlotInfoComponent } from './create-plot-info.component';
import { SharedModule } from 'src/app/shared/shared.module';
import { PageModule } from '@abp/ng.components/page';


@NgModule({
  declarations: [
    CreatePlotInfoComponent,
  ],
  imports: [
    CommonModule,
    CreatePlotInfoRoutingModule,
    SharedModule,
    PageModule,
    

  ]
})
export class CreatePlotInfoModule { }
