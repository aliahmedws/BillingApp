import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { CreateMeterInfoComponent } from './create-meter-info.component';

const routes: Routes = [{ path: '', component: CreateMeterInfoComponent }];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class CreateMeterInfoRoutingModule { }
