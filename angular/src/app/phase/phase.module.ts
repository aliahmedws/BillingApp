import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { PhaseRoutingModule } from './phase-routing.module';
import { PhaseComponent } from './phase.component';
import { SharedModule } from '../shared/shared.module';
import { PageModule } from '@abp/ng.components/page';


@NgModule({
  declarations: [
    PhaseComponent
  ],
  imports: [
    CommonModule,
    PhaseRoutingModule,
    SharedModule,
    PageModule
  ]
})
export class PhaseModule { }
