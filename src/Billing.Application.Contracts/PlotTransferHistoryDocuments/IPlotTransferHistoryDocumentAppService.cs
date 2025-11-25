using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace Billing.PlotTransferHistoryDocuments;

public interface IPlotTransferHistoryDocumentAppService : IApplicationService
{
    Task<PlotTransferHistoryDocumentDto> UploadAsync(IFormFile file, CreatePlotTransferHistoryDocumentDto input);
    Task DeleteAsync(Guid id);
    Task<PlotTransferHistoryDocumentDto> UpdateAsync(Guid id, UpdatePlotTransferHistoryDocumentDto input);
}
