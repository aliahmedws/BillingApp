import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { TarrifSlabRoutingModule } from './tarrif-slab-routing.module';
import { TarrifSlabComponent } from './tarrif-slab.component';


@NgModule({
  declarations: [
    TarrifSlabComponent
  ],
  imports: [
    CommonModule,
    TarrifSlabRoutingModule
  ]
})
export class TarrifSlabModule { }
