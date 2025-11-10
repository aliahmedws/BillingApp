import type { PlotType } from '../plot-types/plot-type.enum';
import type { PlotStatus } from './plot-status.enum';
import type { EntityDto, PagedAndSortedResultRequestDto } from '@abp/ng.core';

export interface CreatePlotInfoDto {
  plotNo: string;
  plotType: PlotType;
  streetNo: string;
  plotSizeId: string;
  status: PlotStatus;
  blockId: string;
  consumerId?: string;
  phaseId: string;
  remarks?: string;
}

export interface GetPlotInfoListDto extends PagedAndSortedResultRequestDto {
  filter?: string;
  plotNo?: string;
  streetNo?: string;
  blockId?: string;
  phaseId?: string;
  plotSizeId?: string;
  status?: PlotStatus;
  consumerId?: string;
}

export interface PlotInfoDto extends EntityDto<string> {
  plotNo?: string;
  plotType?: PlotType;
  streetNo?: string;
  plotSizeId?: string;
  status?: PlotStatus;
  blockId?: string;
  consumerId?: string;
  phaseId?: string;
  remarks?: string;
  blockName?: string;
  phaseName?: string;
  plotSizeName?: string;
  consumerFullName?: string;
}

export interface PlotInfoLookupDto {
  id?: string;
  plotNo?: string;
  consumerId?: string;
  consumerName?: string;
}

export interface UpdatePlotInfoDto {
  plotNo: string;
  plotType: PlotType;
  streetNo: string;
  plotSizeId: string;
  status: PlotStatus;
  blockId: string;
  consumerId?: string;
  phaseId: string;
  remarks?: string;
}
