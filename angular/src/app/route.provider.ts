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
      requiredPolicy: 'Billing.Phases',
    },
     {
      path: '/blocks',
      name: '::Menu:Block',
      parentName: '::Menu:SocietySetup',
      iconClass: 'fas fa-border-all',
      layout: eLayoutType.application,
      requiredPolicy: 'Billing.Blocks',
    },
    {
      path: '/plotSizes',
      name: '::Menu:PlotSize',
      parentName: '::Menu:SocietySetup',
      iconClass: 'fas fa-ruler-combined',
      layout: eLayoutType.application,
      requiredPolicy: 'Billing.PlotSizes',
    },
    {
      path: '/plotTypes',
      name: '::Menu:PlotType',
      parentName: '::Menu:SocietySetup',
      iconClass: 'fas fa-th-large',
      layout: eLayoutType.application,
    },
  ]);
}
