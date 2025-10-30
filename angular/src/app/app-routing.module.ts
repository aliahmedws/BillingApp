import { authGuard, permissionGuard } from '@abp/ng.core';
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

const routes: Routes = [
  {
    path: '',
    pathMatch: 'full',
    loadChildren: () => import('./home/home.module').then(m => m.HomeModule),
  },
  {
    path: 'account',
    loadChildren: () => import('@abp/ng.account').then(m => m.AccountModule.forLazy()),
  },
  {
    path: 'identity',
    loadChildren: () => import('@abp/ng.identity').then(m => m.IdentityModule.forLazy()),
  },
  {
    path: 'tenant-management',
    loadChildren: () =>
      import('@abp/ng.tenant-management').then(m => m.TenantManagementModule.forLazy()),
  },
  {
    path: 'setting-management',
    loadChildren: () =>
      import('@abp/ng.setting-management').then(m => m.SettingManagementModule.forLazy()),
  },
  { path: 'phases', loadChildren: () => import('./phase/phase.module').then(m => m.PhaseModule) },
  { path: 'plotTypes', loadChildren: () => import('./plot-type/plot-type.module').then(m => m.PlotTypeModule) },
  { path: 'blocks', loadChildren: () => import('./block/block.module').then(m => m.BlockModule) },
  { path: 'plotSizes', loadChildren: () => import('./plot-size/plot-size.module').then(m => m.PlotSizeModule) },
  { path: 'consumerPersonalInfos', loadChildren: () => import('./consumer-personal-info/consumer-personal-info.module').then(m => m.ConsumerPersonalInfoModule) },
  { path: 'consumerPersonalInfoCreate', loadChildren: () => import('./consumer-personal-info/consumer-personal-info-create/consumer-personal-info-create.module').then(m => m.ConsumerPersonalInfoCreateModule) },
  { path: 'consumerDocuments', loadChildren: () => import('./consumer-document/consumer-document.module').then(m => m.ConsumerDocumentModule) },
  { path: 'createConsumerDocuments', loadChildren: () => import('./consumer-document/create-consumer-document/create-consumer-document.module').then(m => m.CreateConsumerDocumentModule) },
];

@NgModule({
  imports: [RouterModule.forRoot(routes, {})],
  exports: [RouterModule],
})
export class AppRoutingModule {}
