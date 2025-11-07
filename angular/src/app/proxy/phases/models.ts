import type { EntityDto, PagedAndSortedResultRequestDto } from '@abp/ng.core';

export interface CreatePhaseDto {
  phaseCode: string;
  phaseName: string;
  description?: string;
  isActive: boolean;
}

export interface GetPhaseListDto extends PagedAndSortedResultRequestDto {
  filter?: string;
  phaseCode?: string;
  phaseName?: string;
  isActive?: boolean;
  description?: string;
}

export interface PhaseDto extends EntityDto<string> {
  phaseCode?: string;
  phaseName?: string;
  description?: string;
  isActive: boolean;
  creationTime?: string;
  lastModificationTime?: string;
  creatorId?: string;
  creatorName?: string;
  lastModifierId?: string;
  lastModifierName?: string;
}

export interface PhaseLookUp {
  id?: string;
  phaseName?: string;
}

export interface UpdatePhaseDto {
  phaseCode: string;
  phaseName: string;
  description?: string;
  isActive: boolean;
}
