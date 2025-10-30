import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ConsumerPersonalInfoCreateComponent } from './consumer-personal-info-create.component';

const routes: Routes = [{ path: '', component: ConsumerPersonalInfoCreateComponent }];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ConsumerPersonalInfoCreateRoutingModule { }
