import type { CreateSocietyChargeDto, GetSocietyChargeListDto, SocietyChargeDto, UpdateSocietyChargeDto } from './models';
import { RestService, Rest } from '@abp/ng.core';
import type { PagedResultDto } from '@abp/ng.core';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class SocietyChargeService {
  apiName = 'Default';
  

  create = (input: CreateSocietyChargeDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, SocietyChargeDto>({
      method: 'POST',
      url: '/api/app/society-charges',
      body: input,
    },
    { apiName: this.apiName,...config });
  

  delete = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>({
      method: 'DELETE',
      url: `/api/app/society-charges/${id}`,
    },
    { apiName: this.apiName,...config });
  

  get = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, SocietyChargeDto>({
      method: 'GET',
      url: `/api/app/society-charges/${id}`,
    },
    { apiName: this.apiName,...config });
  

  getList = (input: GetSocietyChargeListDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PagedResultDto<SocietyChargeDto>>({
      method: 'GET',
      url: '/api/app/society-charges',
      params: { filter: input.filter, plotSizeId: input.plotSizeId, securityCharges: input.securityCharges, maintenanceCharges: input.maintenanceCharges, waterCharges: input.waterCharges, otherCharges: input.otherCharges, totalSocietyCharges: input.totalSocietyCharges, sorting: input.sorting, skipCount: input.skipCount, maxResultCount: input.maxResultCount },
    },
    { apiName: this.apiName,...config });
  

  update = (id: string, input: UpdateSocietyChargeDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>({
      method: 'PUT',
      url: `/api/app/society-charges/${id}`,
      body: input,
    },
    { apiName: this.apiName,...config });

  constructor(private restService: RestService) {}
}
