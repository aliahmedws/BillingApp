import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { CreatePlotInfoComponent } from './create-plot-info.component';

const routes: Routes = [{ path: '', component: CreatePlotInfoComponent }];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class CreatePlotInfoRoutingModule { }
