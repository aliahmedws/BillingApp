import type { PlotUnit } from './plot-unit.enum';
import type { EntityDto, PagedAndSortedResultRequestDto } from '@abp/ng.core';

export interface CreatePlotSizeDto {
  sizeName: string;
  area: number;
  unit: PlotUnit;
  length?: number;
  width?: number;
  description?: string;
  isActive: boolean;
}

export interface GetPlotSizeListDto extends PagedAndSortedResultRequestDto {
  filter?: string;
  sizeName?: string;
  isActive?: boolean;
  unit?: PlotUnit;
}

export interface PlotSizeDto extends EntityDto<string> {
  sizeName?: string;
  area: number;
  unit?: PlotUnit;
  length?: number;
  width?: number;
  description?: string;
  isActive: boolean;
  creationTime?: string;
}

export interface PlotSizeLookupDto {
  id?: string;
  plotSizeName?: string;
}

export interface UpdatePlotSizeDto {
  sizeName: string;
  area: number;
  unit: PlotUnit;
  length?: number;
  width?: number;
  description?: string;
  isActive: boolean;
}
