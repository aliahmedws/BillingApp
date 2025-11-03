import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { CreateMeterInfoRoutingModule } from './create-meter-info-routing.module';
import { CreateMeterInfoComponent } from './create-meter-info.component';
import { PageModule } from '@abp/ng.components/page';
import { SharedModule } from 'src/app/shared/shared.module';


@NgModule({
  declarations: [
    CreateMeterInfoComponent
  ],
  imports: [
    CommonModule,
    CreateMeterInfoRoutingModule,
    PageModule,
    SharedModule
  ]
})
export class CreateMeterInfoModule { }
