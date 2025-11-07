import type { CreatePlotSizeDto, GetPlotSizeListDto, PlotSizeDto, PlotSizeLookupDto, UpdatePlotSizeDto } from './models';
import { RestService, Rest } from '@abp/ng.core';
import type { PagedResultDto } from '@abp/ng.core';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class PlotSizeService {
  apiName = 'Default';
  

  create = (input: CreatePlotSizeDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PlotSizeDto>({
      method: 'POST',
      url: '/api/app/plot-sizes',
      body: input,
    },
    { apiName: this.apiName,...config });
  

  delete = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>({
      method: 'DELETE',
      url: `/api/app/plot-sizes/${id}`,
    },
    { apiName: this.apiName,...config });
  

  get = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PlotSizeDto>({
      method: 'GET',
      url: `/api/app/plot-sizes/${id}`,
    },
    { apiName: this.apiName,...config });
  

  getList = (input: GetPlotSizeListDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PagedResultDto<PlotSizeDto>>({
      method: 'GET',
      url: '/api/app/plot-sizes',
      params: { filter: input.filter, sizeName: input.sizeName, isActive: input.isActive, unit: input.unit, sorting: input.sorting, skipCount: input.skipCount, maxResultCount: input.maxResultCount },
    },
    { apiName: this.apiName,...config });
  

  getPlotSizeLookup = (config?: Partial<Rest.Config>) =>
    this.restService.request<any, PlotSizeLookupDto[]>({
      method: 'GET',
      url: '/api/app/plot-sizes/plot-size-lookup',
    },
    { apiName: this.apiName,...config });
  

  update = (id: string, input: UpdatePlotSizeDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>({
      method: 'PUT',
      url: `/api/app/plot-sizes/${id}`,
      body: input,
    },
    { apiName: this.apiName,...config });

  constructor(private restService: RestService) {}
}
