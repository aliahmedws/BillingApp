import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { PrintMaintenanceBillComponent } from './print-maintenance-bill.component';

const routes: Routes = [{ path: '', component: PrintMaintenanceBillComponent }];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class PrintMaintenanceBillRoutingModule { }
