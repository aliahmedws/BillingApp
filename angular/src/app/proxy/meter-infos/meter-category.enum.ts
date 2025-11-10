import { mapEnumToOptions } from '@abp/ng.core';

export enum MeterCategory {
  Electric = 1,
  Gas = 2,
  Water = 3,
}

export const meterCategoryOptions = mapEnumToOptions(MeterCategory);
