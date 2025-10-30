import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { BlockRoutingModule } from './block-routing.module';
import { BlockComponent } from './block.component';
import { PageModule } from '@abp/ng.components/page';
import { SharedModule } from '../shared/shared.module';


@NgModule({
  declarations: [
    BlockComponent
  ],
  imports: [
    CommonModule,
    BlockRoutingModule,
    PageModule,
    SharedModule
  ]
})
export class BlockModule { }
