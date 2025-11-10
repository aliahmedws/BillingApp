import type { EntityDto } from '@abp/ng.core';

export interface GovtChargeDto extends EntityDto<string> {
  ed?: number;
  tvFee?: number;
  gst?: number;
  incomeTax?: number;
  extraTax?: number;
  furtherTax?: number;
  njSurcharge?: number;
  salesTax?: number;
  fcSurcharge?: number;
  trSurcharge?: number;
  taxOnFpa?: number;
  totalTaxes?: number;
}

export interface UpdateGovtChargeDto {
  ed?: number;
  tvFee?: number;
  gst?: number;
  incomeTax?: number;
  extraTax?: number;
  furtherTax?: number;
  njSurcharge?: number;
  salesTax?: number;
  fcSurcharge?: number;
  trSurcharge?: number;
  taxOnFpa?: number;
  totalTaxes?: number;
}
