import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ConsumerDocumentComponent } from './consumer-document.component';

const routes: Routes = [{ path: '', component: ConsumerDocumentComponent }];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ConsumerDocumentRoutingModule { }
