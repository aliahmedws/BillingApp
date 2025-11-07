import type { CreatePhaseDto, GetPhaseListDto, PhaseDto, PhaseLookUp, UpdatePhaseDto } from './models';
import { RestService, Rest } from '@abp/ng.core';
import type { PagedResultDto } from '@abp/ng.core';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class PhaseService {
  apiName = 'Default';
  

  create = (input: CreatePhaseDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PhaseDto>({
      method: 'POST',
      url: '/api/app/phases',
      body: input,
    },
    { apiName: this.apiName,...config });
  

  delete = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>({
      method: 'DELETE',
      url: `/api/app/phases/${id}`,
    },
    { apiName: this.apiName,...config });
  

  get = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PhaseDto>({
      method: 'GET',
      url: `/api/app/phases/${id}`,
    },
    { apiName: this.apiName,...config });
  

  getList = (input: GetPhaseListDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PagedResultDto<PhaseDto>>({
      method: 'GET',
      url: '/api/app/phases',
      params: { filter: input.filter, phaseCode: input.phaseCode, phaseName: input.phaseName, isActive: input.isActive, description: input.description, sorting: input.sorting, skipCount: input.skipCount, maxResultCount: input.maxResultCount },
    },
    { apiName: this.apiName,...config });
  

  getPhaseLookUp = (config?: Partial<Rest.Config>) =>
    this.restService.request<any, PhaseLookUp[]>({
      method: 'GET',
      url: '/api/app/phases/phase-lookup',
    },
    { apiName: this.apiName,...config });
  

  update = (id: string, input: UpdatePhaseDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>({
      method: 'PUT',
      url: `/api/app/phases/${id}`,
      body: input,
    },
    { apiName: this.apiName,...config });

  constructor(private restService: RestService) {}
}
