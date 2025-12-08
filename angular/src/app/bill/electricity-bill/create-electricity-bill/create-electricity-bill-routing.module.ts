import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { CreateElectricityBillComponent } from './create-electricity-bill.component';

const routes: Routes = [{ path: '', component: CreateElectricityBillComponent }];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class CreateElectricityBillRoutingModule { }
