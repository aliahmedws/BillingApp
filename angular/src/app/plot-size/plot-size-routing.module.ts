import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { PlotSizeComponent } from './plot-size.component';

const routes: Routes = [{ path: '', component: PlotSizeComponent }];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class PlotSizeRoutingModule { }
