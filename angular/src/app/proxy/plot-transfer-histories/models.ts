import type { TransferType } from './transfer-type.enum';
import type { EntityDto, PagedAndSortedResultRequestDto } from '@abp/ng.core';

export interface CreatePlotTransferHistoryDto {
  plotId: string;
  fromConsumerId: string;
  toConsumerId: string;
  transferDate: string;
  transferType: TransferType;
  registryNo: string;
  considerationAmount: number;
  remarks?: string;
  approvedByUserId?: string;
  rejectByUserId?: string;
  approvedAt?: string;
  isApproved: boolean;
}

export interface GetPlotTransferHistoryListDto extends PagedAndSortedResultRequestDto {
  filter?: string;
  plotId?: string;
  fromConsumerId?: string;
  toConsumerId?: string;
  transferDate?: string;
  transferType?: TransferType;
  registryNo?: string;
  approvedByUserId?: string;
  approvedAt?: string;
  isApproved?: boolean;
}

export interface PlotTransferHistoryDto extends EntityDto<string> {
  plotId?: string;
  fromConsumerId?: string;
  toConsumerId?: string;
  transferDate?: string;
  transferType?: TransferType;
  registryNo?: string;
  considerationAmount: number;
  remarks?: string;
  approvedByUserId?: string;
  rejectByUserId?: string;
  approvedAt?: string;
  isApproved: boolean;
  fromConsumerName?: string;
  toConsumerName?: string;
  plotNo?: string;
  approvedByUserName?: string;
  rejectByUserName?: string;
}

export interface UpdatePlotTransferHistoryDto {
  plotId: string;
  fromConsumerId: string;
  toConsumerId: string;
  transferDate: string;
  transferType: TransferType;
  registryNo: string;
  considerationAmount: number;
  remarks?: string;
  approvedByUserId?: string;
  rejectByUserId?: string;
  approvedAt?: string;
  isApproved: boolean;
}
