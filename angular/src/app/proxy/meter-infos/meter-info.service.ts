import type { CreateMeterInfoDto, GetMeterInfoListDto, MeterInfoDto, UpdateMeterInfoDto } from './models';
import { RestService, Rest } from '@abp/ng.core';
import type { PagedResultDto } from '@abp/ng.core';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class MeterInfoService {
  apiName = 'Default';
  

  create = (input: CreateMeterInfoDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, MeterInfoDto>({
      method: 'POST',
      url: '/api/app/meter-infos',
      body: input,
    },
    { apiName: this.apiName,...config });
  

  delete = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>({
      method: 'DELETE',
      url: `/api/app/meter-infos/${id}`,
    },
    { apiName: this.apiName,...config });
  

  get = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, MeterInfoDto>({
      method: 'GET',
      url: `/api/app/meter-infos/${id}`,
    },
    { apiName: this.apiName,...config });
  

  getList = (input: GetMeterInfoListDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PagedResultDto<MeterInfoDto>>({
      method: 'GET',
      url: '/api/app/meter-infos',
      params: { filter: input.filter, meterNo: input.meterNo, meterType: input.meterType, meterCategory: input.meterCategory, meterStatus: input.meterStatus, installationDate: input.installationDate, phaseId: input.phaseId, plotId: input.plotId, meterOwnerId: input.meterOwnerId, sorting: input.sorting, skipCount: input.skipCount, maxResultCount: input.maxResultCount },
    },
    { apiName: this.apiName,...config });
  

  update = (id: string, input: UpdateMeterInfoDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>({
      method: 'PUT',
      url: `/api/app/meter-infos/${id}`,
      body: input,
    },
    { apiName: this.apiName,...config });

  constructor(private restService: RestService) {}
}
