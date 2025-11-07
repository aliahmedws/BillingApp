import { mapEnumToOptions } from '@abp/ng.core';

export enum DocumentType {
  CNIC = 1,
  Passport = 2,
  UtilityBill = 3,
  DrivingLicense = 4,
  Other = 5,
}

export const documentTypeOptions = mapEnumToOptions(DocumentType);
