import type { EntityDto, PagedAndSortedResultRequestDto } from '@abp/ng.core';

export interface CreateTarrifSlabDto {
  lowerSlab: number;
  upperSlab?: number;
  unitPrice: number;
}

export interface GetTarrifSlabLIstDto extends PagedAndSortedResultRequestDto {
  filter?: string;
  lowerSlab?: number;
  upperSlab?: number;
  unitPrice?: number;
}

export interface TarrifSlabDto extends EntityDto<string> {
  lowerSlab: number;
  upperSlab?: number;
  unitPrice: number;
}

export interface UpdateTarrifSlabDto {
  lowerSlab: number;
  upperSlab?: number;
  unitPrice: number;
}
