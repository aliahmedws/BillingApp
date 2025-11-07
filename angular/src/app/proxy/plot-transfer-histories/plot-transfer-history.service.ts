import type { CreatePlotTransferHistoryDto, GetPlotTransferHistoryListDto, PlotTransferHistoryDto, UpdatePlotTransferHistoryDto } from './models';
import { RestService, Rest } from '@abp/ng.core';
import type { PagedResultDto } from '@abp/ng.core';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class PlotTransferHistoryService {
  apiName = 'Default';
  

  approve = (transferId: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>({
      method: 'POST',
      url: `/api/app/plot-transfer-histories/${transferId}/approve`,
    },
    { apiName: this.apiName,...config });
  

  create = (input: CreatePlotTransferHistoryDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PlotTransferHistoryDto>({
      method: 'POST',
      url: '/api/app/plot-transfer-histories',
      body: input,
    },
    { apiName: this.apiName,...config });
  

  delete = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>({
      method: 'DELETE',
      url: `/api/app/plot-transfer-histories/${id}`,
    },
    { apiName: this.apiName,...config });
  

  get = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PlotTransferHistoryDto>({
      method: 'GET',
      url: `/api/app/plot-transfer-histories/${id}`,
    },
    { apiName: this.apiName,...config });
  

  getList = (input: GetPlotTransferHistoryListDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PagedResultDto<PlotTransferHistoryDto>>({
      method: 'GET',
      url: '/api/app/plot-transfer-histories',
      params: { filter: input.filter, plotId: input.plotId, fromConsumerId: input.fromConsumerId, toConsumerId: input.toConsumerId, transferDate: input.transferDate, transferType: input.transferType, registryNo: input.registryNo, approvedByUserId: input.approvedByUserId, approvedAt: input.approvedAt, isApproved: input.isApproved, sorting: input.sorting, skipCount: input.skipCount, maxResultCount: input.maxResultCount },
    },
    { apiName: this.apiName,...config });
  

  reject = (id: string, remarks?: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>({
      method: 'POST',
      url: `/api/app/plot-transfer-histories/${id}/reject`,
      body: remarks,
    },
    { apiName: this.apiName,...config });
  

  update = (id: string, input: UpdatePlotTransferHistoryDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>({
      method: 'PUT',
      url: `/api/app/plot-transfer-histories/${id}`,
      body: input,
    },
    { apiName: this.apiName,...config });

  constructor(private restService: RestService) {}
}
