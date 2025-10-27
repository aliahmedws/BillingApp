import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { PhaseComponent } from './phase.component';

const routes: Routes = [{ path: '', component: PhaseComponent }];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class PhaseRoutingModule { }
