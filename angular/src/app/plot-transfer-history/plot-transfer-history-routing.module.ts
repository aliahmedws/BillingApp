import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { PlotTransferHistoryComponent } from './plot-transfer-history.component';

const routes: Routes = [{ path: '', component: PlotTransferHistoryComponent }];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class PlotTransferHistoryRoutingModule { }
