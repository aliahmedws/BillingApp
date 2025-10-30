import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ConsumerPersonalInfoComponent } from './consumer-personal-info.component';

const routes: Routes = [{ path: '', component: ConsumerPersonalInfoComponent }];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ConsumerPersonalInfoRoutingModule { }
