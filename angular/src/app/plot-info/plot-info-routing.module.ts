import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { PlotInfoComponent } from './plot-info.component';

const routes: Routes = [{ path: '', component: PlotInfoComponent }];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class PlotInfoRoutingModule { }
