import { NgModule } from '@angular/core';
import { TarrifSlabRoutingModule } from './tarrif-slab-routing.module';
import { TarrifSlabComponent } from './tarrif-slab.component';
import { SharedModule } from '../shared/shared.module';
import { PageModule } from '@abp/ng.components/page';


@NgModule({
  declarations: [TarrifSlabComponent],
  imports: [SharedModule, TarrifSlabRoutingModule, PageModule],
})
export class TarrifSlabModule { }
