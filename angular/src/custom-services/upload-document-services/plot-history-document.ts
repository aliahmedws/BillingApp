import { Rest, RestService } from "@abp/ng.core";
import { Injectable } from "@angular/core";
import { PlotTransferHistoryDocumentDto } from "../../app/proxy/plot-transfer-history-documents";

@Injectable({
    providedIn: 'root'
})
export class CustomPlotHistoryDocumentService {
    apiName = 'Default';

    constructor(private restService: RestService) { }

    uploadFormData = (formData: FormData, config?: Partial<Rest.Config>) => {
        return this.restService.request<any, PlotTransferHistoryDocumentDto>(
            {
                method: 'POST',
                url: '/api/app/plot-transfer-history-documents/upload',
                body: formData,
            },
            { apiName: this.apiName, ...config }
        );
    }
} 