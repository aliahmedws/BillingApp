import { Injectable } from '@angular/core';
import { RestService, Rest, mapEnumToOptions } from '@abp/ng.core';
import { MeterDocumentDto, MeterDocumentType } from 'src/app/proxy/meter-documents';

@Injectable({
  providedIn: 'root',
})
export class OwnMeterDocumentService {
  apiName = 'Default';

  constructor(private restService: RestService) {}

uploadFormData = (formData: FormData, config?: Partial<Rest.Config>) => {
  return this.restService.request<any, MeterDocumentDto>(
    {
      method: 'POST',
      url: '/api/app/meter-documents/upload',
      body: formData,
    },
    { apiName: this.apiName, ...config }
  );
};

}

export const meterDocumentTypeOptions = mapEnumToOptions(MeterDocumentType);
