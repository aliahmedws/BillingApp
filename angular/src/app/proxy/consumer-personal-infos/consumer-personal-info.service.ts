import type { ConsumerPersonalInfoDto, ConsumerPersonalInfoLookupDto, CreateConsumerPersonalInfoDto, GetConsumerPersonalInfoListDto, UpdateConsumerPersonalInfoDto } from './models';
import { RestService, Rest } from '@abp/ng.core';
import type { PagedResultDto } from '@abp/ng.core';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class ConsumerPersonalInfoService {
  apiName = 'Default';
  

  consumerPersonalInfoLookup = (config?: Partial<Rest.Config>) =>
    this.restService.request<any, ConsumerPersonalInfoLookupDto[]>({
      method: 'GET',
      url: '/api/app/consumer-personal-infos/consumer-lookup',
    },
    { apiName: this.apiName,...config });
  

  create = (input: CreateConsumerPersonalInfoDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, ConsumerPersonalInfoDto>({
      method: 'POST',
      url: '/api/app/consumer-personal-infos',
      body: input,
    },
    { apiName: this.apiName,...config });
  

  delete = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>({
      method: 'DELETE',
      url: `/api/app/consumer-personal-infos/${id}`,
    },
    { apiName: this.apiName,...config });
  

  get = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, ConsumerPersonalInfoDto>({
      method: 'GET',
      url: `/api/app/consumer-personal-infos/${id}`,
    },
    { apiName: this.apiName,...config });
  

  getList = (input: GetConsumerPersonalInfoListDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PagedResultDto<ConsumerPersonalInfoDto>>({
      method: 'GET',
      url: '/api/app/consumer-personal-infos',
      params: { filter: input.filter, firstName: input.firstName, lastName: input.lastName, cnic: input.cnic, gender: input.gender, sorting: input.sorting, skipCount: input.skipCount, maxResultCount: input.maxResultCount },
    },
    { apiName: this.apiName,...config });
  

  update = (id: string, input: UpdateConsumerPersonalInfoDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>({
      method: 'PUT',
      url: `/api/app/consumer-personal-infos/${id}`,
      body: input,
    },
    { apiName: this.apiName,...config });

  constructor(private restService: RestService) {}
}
