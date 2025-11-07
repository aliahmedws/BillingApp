import type { CreatePlotInfoDto, GetPlotInfoListDto, PlotInfoDto, PlotInfoLookupDto, UpdatePlotInfoDto } from './models';
import { RestService, Rest } from '@abp/ng.core';
import type { PagedResultDto } from '@abp/ng.core';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class PlotInfoService {
  apiName = 'Default';
  

  create = (input: CreatePlotInfoDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PlotInfoDto>({
      method: 'POST',
      url: '/api/app/plot-infos',
      body: input,
    },
    { apiName: this.apiName,...config });
  

  delete = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>({
      method: 'DELETE',
      url: `/api/app/plot-infos/${id}`,
    },
    { apiName: this.apiName,...config });
  

  get = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PlotInfoDto>({
      method: 'GET',
      url: `/api/app/plot-infos/${id}`,
    },
    { apiName: this.apiName,...config });
  

  getList = (input: GetPlotInfoListDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PagedResultDto<PlotInfoDto>>({
      method: 'GET',
      url: '/api/app/plot-infos',
      params: { filter: input.filter, plotNo: input.plotNo, streetNo: input.streetNo, blockId: input.blockId, phaseId: input.phaseId, plotSizeId: input.plotSizeId, status: input.status, consumerId: input.consumerId, sorting: input.sorting, skipCount: input.skipCount, maxResultCount: input.maxResultCount },
    },
    { apiName: this.apiName,...config });
  

  getPlotLookUp = (config?: Partial<Rest.Config>) =>
    this.restService.request<any, PlotInfoLookupDto[]>({
      method: 'GET',
      url: '/api/app/plot-infos/plotInfo-lookup',
    },
    { apiName: this.apiName,...config });
  

  getPlotOwner = (plotId: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PlotInfoLookupDto>({
      method: 'GET',
      url: `/api/app/plot-infos/get-plot-owner/${plotId}`,
    },
    { apiName: this.apiName,...config });
  

  update = (id: string, input: UpdatePlotInfoDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>({
      method: 'PUT',
      url: `/api/app/plot-infos/${id}`,
      body: input,
    },
    { apiName: this.apiName,...config });

  constructor(private restService: RestService) {}
}
