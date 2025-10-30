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
  { path: 'govtcharges', loadChildren: () => import('./govtcharge/govtcharge.module').then(m => m.GovtchargeModule) },
  { path: 'iescocharges', loadChildren: () => import('./iescocharge/iescocharge.module').then(m => m.IescochargeModule) },
  { path: 'societycharges', loadChildren: () => import('./societycharge/societycharge.module').then(m => m.SocietychargeModule) },
];

@NgModule({
  imports: [RouterModule.forRoot(routes, {})],
  exports: [RouterModule],
})
export class AppRoutingModule {}
