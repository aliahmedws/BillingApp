import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { CreatePlotTransferHistoryComponent } from './create-plot-transfer-history.component';

const routes: Routes = [{ path: '', component: CreatePlotTransferHistoryComponent }];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class CreatePlotTransferHistoryRoutingModule { }
