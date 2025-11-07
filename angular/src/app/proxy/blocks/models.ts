import type { EntityDto, PagedAndSortedResultRequestDto } from '@abp/ng.core';

export interface BlockDto extends EntityDto<string> {
  blockCode?: string;
  blockName?: string;
  phaseId?: string;
  phaseName?: string;
  description?: string;
  isActive: boolean;
  creationTime?: string;
}

export interface BlockLookupDto {
  id?: string;
  blockCode?: string;
  blockName?: string;
}

export interface CreateBlockDto {
  blockCode: string;
  blockName: string;
  phaseId: string;
  description?: string;
  isActive: boolean;
}

export interface GetBlockListDto extends PagedAndSortedResultRequestDto {
  filter?: string;
  blockCode?: string;
  blockName?: string;
  isActive?: boolean;
  description?: string;
  phaseId?: string;
}

export interface UpdateBlockDto {
  blockCode: string;
  blockName: string;
  phaseId: string;
  description?: string;
  isActive: boolean;
}
