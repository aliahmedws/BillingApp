import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { MeterInfoComponent } from './meter-info.component';

const routes: Routes = [{ path: '', component: MeterInfoComponent }];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class MeterInfoRoutingModule { }
