import { RoutesService, eLayoutType } from '@abp/ng.core';
import { inject, provideAppInitializer } from '@angular/core';

export const APP_ROUTE_PROVIDER = [
  provideAppInitializer(() => {
    configureRoutes();
  }),
];

function configureRoutes() {
  const routes = inject(RoutesService);
  routes.add([
      {
        path: '/',
        name: '::Menu:Home',
        iconClass: 'fas fa-home',
        order: 1,
        layout: eLayoutType.application,
      },
     {
      path: '/society-setup',
      name: '::Menu:SocietySetup',
      iconClass: 'fas fa-city',
      order: 2,
      layout: eLayoutType.application,
    },
    // Child menus (grouped under Society Setup)
    {
      path: '/phases',
      name: '::Menu:Phase',
      parentName: '::Menu:SocietySetup',
      iconClass: 'fas fa-layer-group',
      layout: eLayoutType.application,
    },
  ]);
}
