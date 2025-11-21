using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace Billing.PlotDocuments;

public interface IPlotDocumentAppService : IApplicationService
{
    Task<PlotDocumentDto> UploadAsync(IFormFile file, CreatePlotDocumentDto input);
    Task DeleteAsync(Guid id);
    Task<PlotDocumentDto> UpdateAsync(Guid id, UpdatePlotDocumentDto input);
}
