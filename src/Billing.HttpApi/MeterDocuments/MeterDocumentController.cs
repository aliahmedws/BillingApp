using Asp.Versioning;
using Billing.MeterDocuments;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc;

[RemoteService(IsEnabled = true)]
[ControllerName("MeterDocuments")]
[Area("app")]
[Route("api/app/meter-documents")]
public class MeterDocumentController : AbpController, IMeterDocumentAppService
{
    private readonly IMeterDocumentAppService _appService;

    public MeterDocumentController(IMeterDocumentAppService appService)
    {
        _appService = appService;
    }

    [HttpDelete("{id}")]
    public async Task DeleteAsync(Guid id)
    {
        await _appService.DeleteAsync(id);
    }

    [HttpPut("update/{id}")]
    public async Task<MeterDocumentDto> UpdateAsync(Guid id, UpdateMeterDocumentDto input)
    {
        return await _appService.UpdateAsync(id, input);
    }

    [ApiExplorerSettings(IgnoreApi = true)]
    [HttpPost("upload")]
    public async Task<MeterDocumentDto> UploadAsync([FromForm] IFormFile file, [FromForm] CreateMeterDocumentDto input)
    {
        return await _appService.UploadAsync(file, input);
    }


}

