import { mapEnumToOptions } from '@abp/ng.core';

export enum MeterStatus {
  Active = 1,
  Inactive = 2,
  Faulty = 3,
  UnderMaintenance = 4,
  Disconnected = 5,
  PendingInstallation = 6,
}

export const meterStatusOptions = mapEnumToOptions(MeterStatus);
