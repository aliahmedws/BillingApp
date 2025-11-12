import { Injectable } from '@angular/core';
import { RestService, Rest, mapEnumToOptions } from '@abp/ng.core';
import { MeterDocumentDto } from 'src/app/proxy/meter-documents';

@Injectable({
  providedIn: 'root',
})
export class OwnMeterDocumentService {
  apiName = 'Default';

  constructor(private restService: RestService) {}

//   upload = (
//     meterId: string,
//     file: File,
//     type: MeterDocumentType,
//     description?: string,
//     config?: Partial<Rest.Config>
//   ) => {
//     const formData = new FormData();
//     formData.append('meterId', meterId);
//     formData.append('file', file);
//     formData.append('type', type.toString());
//     if (description) {
//       formData.append('description', description);
//     }

//     return this.restService.request<any, MeterDocumentDto>(
//       {
//         method: 'POST',
//         url: '/api/app/meter-documents/upload',
//         body: formData
//       },
//       { apiName: this.apiName, ...config }
//     );
//   };

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


export enum MeterDocumentType {
  InstallationPhoto = 1,
  InspectionReport = 2,
  ReadingEvidence = 3,
  MaintenanceDocument = 4,
  Other = 5
}


interface UploadedDocumentViewModel {
  id: string;
  meterId: string;
  documentType: number;
  description?: string;
  fileName: string;
  fileUrl: string;
}

export const meterDocumentTypeOptions = mapEnumToOptions(MeterDocumentType);
