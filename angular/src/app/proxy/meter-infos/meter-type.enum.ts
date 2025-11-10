import { mapEnumToOptions } from '@abp/ng.core';

export enum MeterType {
  SinglePhase = 1,
  ThreePhase = 2,
  SmartMeter = 3,
  Prepaid = 4,
}

export const meterTypeOptions = mapEnumToOptions(MeterType);
