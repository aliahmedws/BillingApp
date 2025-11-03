import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { MeterInfoRoutingModule } from './meter-info-routing.module';
import { MeterInfoComponent } from './meter-info.component';
import { SharedModule } from '../shared/shared.module';
import { PageModule } from '@abp/ng.components/page';


@NgModule({
  declarations: [
    MeterInfoComponent
  ],
  imports: [
    CommonModule,
    MeterInfoRoutingModule,
    SharedModule,
    PageModule
  ]
})
export class MeterInfoModule { }
