import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { PlotTypeComponent } from './plot-type.component';

const routes: Routes = [{ path: '', component: PlotTypeComponent }];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class PlotTypeRoutingModule { }
