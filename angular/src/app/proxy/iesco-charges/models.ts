import type { EntityDto } from '@abp/ng.core';

export interface IescoChargeDto extends EntityDto<string> {
  totalEnergyCharges?: number;
  iescoFixCharges?: number;
  serviceRent?: number;
  varFpa?: number;
  qtrTariffAdj?: number;
  totalIescoCharges?: number;
}

export interface UpdateIescoChargeDto {
  totalEnergyCharges?: number;
  iescoFixCharges?: number;
  serviceRent?: number;
  varFpa?: number;
  qtrTariffAdj?: number;
  totalIescoCharges?: number;
}
