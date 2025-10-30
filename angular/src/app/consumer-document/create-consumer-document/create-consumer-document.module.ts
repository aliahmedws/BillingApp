import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { CreateConsumerDocumentRoutingModule } from './create-consumer-document-routing.module';
import { CreateConsumerDocumentComponent } from './create-consumer-document.component';
import { PageModule } from '@abp/ng.components/page';
import { SharedModule } from 'src/app/shared/shared.module';


@NgModule({
  declarations: [
    CreateConsumerDocumentComponent
  ],
  imports: [
    CommonModule,
    CreateConsumerDocumentRoutingModule,
    SharedModule,
    PageModule
  ]
})
export class CreateConsumerDocumentModule { }
