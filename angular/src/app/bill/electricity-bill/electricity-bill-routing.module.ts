import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ElectricityBillComponent } from './electricity-bill.component';

const routes: Routes = [{ path: '', component: ElectricityBillComponent }];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ElectricityBillRoutingModule { }
