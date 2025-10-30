import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { SocietychargeComponent } from './societycharge.component';

const routes: Routes = [{ path: '', component: SocietychargeComponent }];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class SocietychargeRoutingModule { }
