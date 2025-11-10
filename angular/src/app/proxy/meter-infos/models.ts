import type { MeterType } from './meter-type.enum';
import type { MeterCategory } from './meter-category.enum';
import type { MeterStatus } from './meter-status.enum';
import type { EntityDto, PagedAndSortedResultRequestDto } from '@abp/ng.core';

export interface CreateMeterInfoDto {
  meterNo: string;
  meterType: MeterType;
  meterCategory: MeterCategory;
  meterStatus: MeterStatus;
  installationDate: string;
  initialReading: number;
  phaseId: string;
  plotId: string;
  meterOwnerId: string;
  remarks?: string;
}

export interface GetMeterInfoListDto extends PagedAndSortedResultRequestDto {
  filter?: string;
  meterNo?: string;
  meterType?: MeterType;
  meterCategory?: MeterCategory;
  meterStatus?: MeterStatus;
  installationDate?: string;
  phaseId?: string;
  plotId?: string;
  meterOwnerId?: string;
}

export interface MeterInfoDto extends EntityDto<string> {
  meterNo?: string;
  meterType?: MeterType;
  meterCategory?: MeterCategory;
  meterStatus?: MeterStatus;
  installationDate?: string;
  initialReading: number;
  phaseId?: string;
  phaseName?: string;
  plotId?: string;
  meterOwnerId?: string;
  meterOwnerName?: string;
  plotNo?: string;
  remarks?: string;
}

export interface UpdateMeterInfoDto {
  meterNo: string;
  meterType: MeterType;
  meterCategory: MeterCategory;
  meterStatus: MeterStatus;
  installationDate: string;
  initialReading: number;
  phaseId: string;
  plotId: string;
  meterOwnerId: string;
  remarks?: string;
}
