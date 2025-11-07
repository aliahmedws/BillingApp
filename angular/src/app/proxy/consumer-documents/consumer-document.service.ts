import type { ConsumerDocumentDto, CreateConsumerDocumentDto, GetConsumerDocumentListDto } from './models';
import { RestService, Rest } from '@abp/ng.core';
import type { PagedResultDto } from '@abp/ng.core';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class ConsumerDocumentService {
  apiName = 'Default';
  

  create = (input: CreateConsumerDocumentDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, ConsumerDocumentDto>({
      method: 'POST',
      url: '/api/app/consumer-documents',
      body: input,
    },
    { apiName: this.apiName,...config });
  

  delete = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>({
      method: 'DELETE',
      url: `/api/app/consumer-documents/${id}`,
    },
    { apiName: this.apiName,...config });
  

  get = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, ConsumerDocumentDto>({
      method: 'GET',
      url: `/api/app/consumer-documents/${id}`,
    },
    { apiName: this.apiName,...config });
  

  getList = (input: GetConsumerDocumentListDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PagedResultDto<ConsumerDocumentDto>>({
      method: 'GET',
      url: '/api/app/consumer-documents',
      params: { filter: input.filter, consumerId: input.consumerId, documentType: input.documentType, isVerified: input.isVerified, issueDate: input.issueDate, expireDate: input.expireDate, sorting: input.sorting, skipCount: input.skipCount, maxResultCount: input.maxResultCount },
    },
    { apiName: this.apiName,...config });
  

  update = (id: string, input: CreateConsumerDocumentDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>({
      method: 'PUT',
      url: `/api/app/consumer-documents/${id}`,
      body: input,
    },
    { apiName: this.apiName,...config });

  constructor(private restService: RestService) {}
}
