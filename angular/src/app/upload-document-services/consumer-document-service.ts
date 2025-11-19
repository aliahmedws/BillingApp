import { Rest, RestService } from "@abp/ng.core";
import { Injectable } from "@angular/core";
import { ConsumerDocumentDto } from "../proxy/consumer-documents";

@Injectable({
    providedIn: 'root'
})
export class CustomConsumerDocumentService {
    apiName = 'Default';

    constructor(private restService: RestService) {}

    uploadFormData = (formData: FormData, config?: Partial<Rest.Config>) => {
        return this.restService.request<any, ConsumerDocumentDto>(
            {
                method: 'POST',
                url: '/api/app/consumer-documents/upload',
                body: formData,
            },
            { apiName: this.apiName, ...config }
        );
    }

}