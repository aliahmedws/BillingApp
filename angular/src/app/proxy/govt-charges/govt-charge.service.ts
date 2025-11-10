import type { GovtChargeDto, UpdateGovtChargeDto } from './models';
import { RestService, Rest } from '@abp/ng.core';
import type { PagedResultDto } from '@abp/ng.core';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class GovtChargeService {
  apiName = 'Default';
  

  get = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, GovtChargeDto>({
      method: 'GET',
      url: `/api/app/govt-charges/${id}`,
    },
    { apiName: this.apiName,...config });
  

  getList = (config?: Partial<Rest.Config>) =>
    this.restService.request<any, PagedResultDto<GovtChargeDto>>({
      method: 'GET',
      url: '/api/app/govt-charges',
    },
    { apiName: this.apiName,...config });
  

  update = (id: string, input: UpdateGovtChargeDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>({
      method: 'PUT',
      url: `/api/app/govt-charges/${id}`,
      body: input,
    },
    { apiName: this.apiName,...config });

  constructor(private restService: RestService) {}
}
