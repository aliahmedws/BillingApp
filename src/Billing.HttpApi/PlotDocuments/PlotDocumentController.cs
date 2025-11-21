using Asp.Versioning;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc;

namespace Billing.PlotDocuments;

[RemoteService(IsEnabled = true)]
[ControllerName("PlotDocuments")]
[Area("app")]
[Route("api/app/plot-documents")]
public class PlotDocumentController : AbpController, IPlotDocumentAppService
{
    private readonly IPlotDocumentAppService _appService;

    public PlotDocumentController(IPlotDocumentAppService appService)
    {
        _appService = appService;
    }

    [HttpDelete("{id}")]
    public async Task DeleteAsync(Guid id)
    {
        await _appService.DeleteAsync(id);
    }

    [HttpPut("update/{id}")]
    public async Task<PlotDocumentDto> UpdateAsync(Guid id, UpdatePlotDocumentDto input)
    {
        return await _appService.UpdateAsync(id, input);
    }

    [ApiExplorerSettings(IgnoreApi = true)]
    [HttpPost("upload")]
    public async Task<PlotDocumentDto> UploadAsync([FromForm] IFormFile file, [FromForm] CreatePlotDocumentDto input)
    {
        return await _appService.UploadAsync(file, input);
    }
}
