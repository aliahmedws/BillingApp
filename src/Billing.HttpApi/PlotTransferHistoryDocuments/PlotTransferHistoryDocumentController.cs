using Asp.Versioning;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc;

namespace Billing.PlotTransferHistoryDocuments;

[RemoteService(IsEnabled = true)]
[ControllerName("PlotTransferHistoryDocuments")]
[Area("app")]
[Route("api/app/plot-transfer-history-documents")]
public class PlotTransferHistoryDocumentController : AbpController, IPlotTransferHistoryDocumentDtoAppService
{
    private readonly IPlotTransferHistoryDocumentDtoAppService _appService;

    public PlotTransferHistoryDocumentController(IPlotTransferHistoryDocumentDtoAppService appService)
    {
        _appService = appService;
    }

    [HttpDelete("{id}")]
    public async Task DeleteAsync(Guid id)
    {
        await _appService.DeleteAsync(id);
    }

    [HttpPut("update/{id}")]
    public async Task<PlotTransferHistoryDocumentDto> UpdateAsync(Guid id, UpdatePlotTransferHistoryDocumentDto input)
    {
        return await _appService.UpdateAsync(id, input);
    }

    [ApiExplorerSettings(IgnoreApi = true)]
    [HttpPost("upload")]
    public async Task<PlotTransferHistoryDocumentDto> UploadAsync(IFormFile file, CreatePlotTransferHistoryDocument input)
    {
        return await _appService.UploadAsync(file, input);
    }
}
