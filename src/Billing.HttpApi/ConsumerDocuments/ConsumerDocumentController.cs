using Asp.Versioning;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc;

namespace Billing.ConsumerDocuments;

[RemoteService(IsEnabled = true)]
[ControllerName("ConsumerDocuments")]
[Area("app")]
[Route("api/app/consumer-documents")]
public class ConsumerDocumentController : AbpController, IConsumerDocumentAppService
{
    private readonly IConsumerDocumentAppService _appService;

    public ConsumerDocumentController(IConsumerDocumentAppService appService)
    {
        _appService = appService;
    }

    [HttpDelete("{id}")]
    public async Task DeleteAsync(Guid id)
    {
        await _appService.DeleteAsync(id);
    }

    [HttpPut("update/{id}")]
    public async Task<ConsumerDocumentDto> UpdateAsync(Guid id, UpdateConsumerDocumentDto input)
    {
        return await _appService.UpdateAsync(id, input);
    }

    [ApiExplorerSettings(IgnoreApi = true)]
    [HttpPost("upload")]
    public async Task<ConsumerDocumentDto> UploadAsync([FromForm] IFormFile file, [FromForm] CreateConsumerDocumentDto input)
    {
        return await _appService.UploadAsync(file, input);
    }
}
