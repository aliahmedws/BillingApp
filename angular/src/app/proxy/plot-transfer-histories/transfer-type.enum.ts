import { mapEnumToOptions } from '@abp/ng.core';

export enum TransferType {
  Sale = 1,
  Gift = 2,
  Inheritance = 3,
  Mortgage = 4,
  Lease = 5,
  Exchange = 6,
  Donation = 7,
  Other = 8,
}

export const transferTypeOptions = mapEnumToOptions(TransferType);
