import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { IescochargeComponent } from './iescocharge.component';

const routes: Routes = [{ path: '', component: IescochargeComponent }];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class IescochargeRoutingModule { }
