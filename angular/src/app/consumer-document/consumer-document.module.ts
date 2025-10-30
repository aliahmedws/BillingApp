import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { ConsumerDocumentRoutingModule } from './consumer-document-routing.module';
import { ConsumerDocumentComponent } from './consumer-document.component';
import { PageModule } from '@abp/ng.components/page';
import { SharedModule } from '../shared/shared.module';


@NgModule({
  declarations: [
    ConsumerDocumentComponent
  ],
  imports: [
    CommonModule,
    ConsumerDocumentRoutingModule,
    PageModule,
    SharedModule
  ]
})
export class ConsumerDocumentModule { }
