import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { ConsumerPersonalInfoRoutingModule } from './consumer-personal-info-routing.module';
import { ConsumerPersonalInfoComponent } from './consumer-personal-info.component';
import { PageModule } from '@abp/ng.components/page';
import { SharedModule } from '../shared/shared.module';
import { ThemeSharedModule } from '@abp/ng.theme.shared';


@NgModule({
  declarations: [
    ConsumerPersonalInfoComponent
  ],
  imports: [
    CommonModule,
    ConsumerPersonalInfoRoutingModule,
    PageModule,
    SharedModule,
    ThemeSharedModule
  ]
})
export class ConsumerPersonalInfoModule { }
