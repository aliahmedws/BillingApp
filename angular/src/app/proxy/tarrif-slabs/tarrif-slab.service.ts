import type { CreateTarrifSlabDto, GetTarrifSlabLIstDto, TarrifSlabDto, UpdateTarrifSlabDto } from './models';
import { RestService, Rest } from '@abp/ng.core';
import type { PagedResultDto } from '@abp/ng.core';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class TarrifSlabService {
  apiName = 'Default';
  

  create = (input: CreateTarrifSlabDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, TarrifSlabDto>({
      method: 'POST',
      url: '/api/app/tarrif-slabs',
      body: input,
    },
    { apiName: this.apiName,...config });
  

  delete = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>({
      method: 'DELETE',
      url: `/api/app/tarrif-slabs/${id}`,
    },
    { apiName: this.apiName,...config });
  

  get = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, TarrifSlabDto>({
      method: 'GET',
      url: `/api/app/tarrif-slabs/${id}`,
    },
    { apiName: this.apiName,...config });
  

  getList = (input: GetTarrifSlabLIstDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PagedResultDto<TarrifSlabDto>>({
      method: 'GET',
      url: '/api/app/tarrif-slabs',
      params: { filter: input.filter, lowerSlab: input.lowerSlab, upperSlab: input.upperSlab, unitPrice: input.unitPrice, sorting: input.sorting, skipCount: input.skipCount, maxResultCount: input.maxResultCount },
    },
    { apiName: this.apiName,...config });
  

  update = (id: string, input: UpdateTarrifSlabDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>({
      method: 'PUT',
      url: `/api/app/tarrif-slabs/${id}`,
      body: input,
    },
    { apiName: this.apiName,...config });

  constructor(private restService: RestService) {}
}
