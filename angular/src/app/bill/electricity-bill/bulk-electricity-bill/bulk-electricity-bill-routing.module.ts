import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { BulkElectricityBillComponent } from './bulk-electricity-bill.component';

const routes: Routes = [{ path: '', component: BulkElectricityBillComponent }];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class BulkElectricityBillRoutingModule { }
