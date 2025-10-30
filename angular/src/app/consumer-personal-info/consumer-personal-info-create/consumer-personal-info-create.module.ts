import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { ConsumerPersonalInfoCreateRoutingModule } from './consumer-personal-info-create-routing.module';
import { ConsumerPersonalInfoCreateComponent } from './consumer-personal-info-create.component';
import { SharedModule } from 'src/app/shared/shared.module';
import { PageModule } from '@abp/ng.components/page';


@NgModule({
  declarations: [
    ConsumerPersonalInfoCreateComponent
  ],
  imports: [
    CommonModule,
    ConsumerPersonalInfoCreateRoutingModule,
    SharedModule,
    PageModule
  ]
})
export class ConsumerPersonalInfoCreateModule { }
