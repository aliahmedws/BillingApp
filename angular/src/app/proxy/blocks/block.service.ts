import type { BlockDto, BlockLookupDto, CreateBlockDto, GetBlockListDto, UpdateBlockDto } from './models';
import { RestService, Rest } from '@abp/ng.core';
import type { PagedResultDto } from '@abp/ng.core';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class BlockService {
  apiName = 'Default';
  

  create = (input: CreateBlockDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, BlockDto>({
      method: 'POST',
      url: '/api/app/blocks',
      body: input,
    },
    { apiName: this.apiName,...config });
  

  delete = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>({
      method: 'DELETE',
      url: `/api/app/blocks/${id}`,
    },
    { apiName: this.apiName,...config });
  

  get = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, BlockDto>({
      method: 'GET',
      url: `/api/app/blocks/${id}`,
    },
    { apiName: this.apiName,...config });
  

  getBlockLookup = (config?: Partial<Rest.Config>) =>
    this.restService.request<any, BlockLookupDto[]>({
      method: 'GET',
      url: '/api/app/blocks/block-lookup',
    },
    { apiName: this.apiName,...config });
  

  getList = (input: GetBlockListDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PagedResultDto<BlockDto>>({
      method: 'GET',
      url: '/api/app/blocks',
      params: { filter: input.filter, blockCode: input.blockCode, blockName: input.blockName, isActive: input.isActive, description: input.description, phaseId: input.phaseId, sorting: input.sorting, skipCount: input.skipCount, maxResultCount: input.maxResultCount },
    },
    { apiName: this.apiName,...config });
  

  update = (id: string, input: UpdateBlockDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>({
      method: 'PUT',
      url: `/api/app/blocks/${id}`,
      body: input,
    },
    { apiName: this.apiName,...config });

  constructor(private restService: RestService) {}
}
