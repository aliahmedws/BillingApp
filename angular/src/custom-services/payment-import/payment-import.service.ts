import { Rest, RestService } from "@abp/ng.core";
import { Injectable } from "@angular/core";

@Injectable({
    providedIn: 'root'
})

export class PaymentImportService {
  apiName = 'Default';

  constructor(private restService: RestService) {}

  importExcelFile(formData: FormData, config?: Partial<Rest.Config>) {
    return this.restService.request<any, void>({
      method: 'POST',
      url: '/api/app/maintenance-payment-history/import',
      body: formData
    }, { apiName: this.apiName, ...config });
  }
}