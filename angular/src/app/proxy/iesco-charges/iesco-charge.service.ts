import type { IescoChargeDto, UpdateIescoChargeDto } from './models';
import { RestService, Rest } from '@abp/ng.core';
import type { PagedResultDto } from '@abp/ng.core';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class IescoChargeService {
  apiName = 'Default';
  

  get = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, IescoChargeDto>({
      method: 'GET',
      url: `/api/app/iesco-charges/${id}`,
    },
    { apiName: this.apiName,...config });
  

  getList = (config?: Partial<Rest.Config>) =>
    this.restService.request<any, PagedResultDto<IescoChargeDto>>({
      method: 'GET',
      url: '/api/app/iesco-charges',
    },
    { apiName: this.apiName,...config });
  

  update = (id: string, input: UpdateIescoChargeDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>({
      method: 'PUT',
      url: `/api/app/iesco-charges/${id}`,
      body: input,
    },
    { apiName: this.apiName,...config });

  constructor(private restService: RestService) {}
}
