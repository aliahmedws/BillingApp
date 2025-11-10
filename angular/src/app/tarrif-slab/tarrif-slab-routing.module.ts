import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { TarrifSlabComponent } from './tarrif-slab.component';

const routes: Routes = [{ path: '', component: TarrifSlabComponent }];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class TarrifSlabRoutingModule { }
