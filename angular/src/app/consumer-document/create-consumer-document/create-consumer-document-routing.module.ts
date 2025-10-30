import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { CreateConsumerDocumentComponent } from './create-consumer-document.component';

const routes: Routes = [{ path: '', component: CreateConsumerDocumentComponent }];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class CreateConsumerDocumentRoutingModule { }
