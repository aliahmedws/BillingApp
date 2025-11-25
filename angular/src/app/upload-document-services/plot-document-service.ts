import { Rest, RestService } from "@abp/ng.core";
import { Injectable } from "@angular/core";
import { PlotDocumentDto } from "../proxy/plot-documents";

@Injectable({
    providedIn: 'root',
})
export class CustomPlotDocumentService {
    apiName = 'Default';

    constructor(private restService: RestService) {}

    uploadFormData = (formData: FormData, config?: Partial<Rest.Config>) => {
        return this.restService.request<any, PlotDocumentDto>(
            {
                method: 'POST',
                url: '/api/app/plot-documents/upload',
                body: formData,
            },
            { apiName: this.apiName, ...config }
        );
    };
}