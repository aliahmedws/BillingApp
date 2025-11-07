import type { EntityDto, PagedAndSortedResultRequestDto } from '@abp/ng.core';

export interface CreateSocietyChargeDto {
  plotSizeId?: string;
  securityCharges?: number;
  maintenanceCharges?: number;
  waterCharges?: number;
  otherCharges?: number;
  totalSocietyCharges?: number;
}

export interface GetSocietyChargeListDto extends PagedAndSortedResultRequestDto {
  filter?: string;
  plotSizeId?: string;
  securityCharges?: number;
  maintenanceCharges?: number;
  waterCharges?: number;
  otherCharges?: number;
  totalSocietyCharges?: number;
}

export interface SocietyChargeDto extends EntityDto<string> {
  plotSizeId?: string;
  sizeName?: string;
  securityCharges?: number;
  maintenanceCharges?: number;
  waterCharges?: number;
  otherCharges?: number;
  totalSocietyCharges?: number;
}

export interface UpdateSocietyChargeDto {
  plotSizeId?: string;
  securityCharges?: number;
  maintenanceCharges?: number;
  waterCharges?: number;
  otherCharges?: number;
  totalSocietyCharges?: number;
}
