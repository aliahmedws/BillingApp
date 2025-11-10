import { mapEnumToOptions } from '@abp/ng.core';

export enum PlotUnit {
  SquareFeet = 1,
  SquareYard = 2,
  SquareMeter = 3,
  Marla = 4,
  Kanal = 5,
  Acre = 6,
}

export const plotUnitOptions = mapEnumToOptions(PlotUnit);
