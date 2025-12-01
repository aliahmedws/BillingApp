import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { CreateMaintenanceBillComponent } from './create-maintenance-bill.component';

const routes: Routes = [{ path: '', component: CreateMaintenanceBillComponent }];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class CreateMaintenanceBillRoutingModule { }
