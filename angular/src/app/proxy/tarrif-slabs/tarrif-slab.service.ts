import type { TarrifSlabDto, UpdateTarrifSlabDto } from './models';
import { RestService, Rest } from '@abp/ng.core';
import type { PagedResultDto } from '@abp/ng.core';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class TarrifSlabService {
  apiName = 'Default';
  

  get = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, TarrifSlabDto>({
      method: 'GET',
      url: `/api/app/tarrif-slabs/${id}`,
    },
    { apiName: this.apiName,...config });
  

  getList = (config?: Partial<Rest.Config>) =>
    this.restService.request<any, PagedResultDto<TarrifSlabDto>>({
      method: 'GET',
      url: '/api/app/tarrif-slabs',
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
