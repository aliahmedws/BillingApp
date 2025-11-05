using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc;

namespace Billing.ConsumerDocuments;

[RemoteService(IsEnabled = true)]
[ControllerName("ConsumerDocuments")]
[Area("app")]
[Route("api/app/consumer-documents")]
public class ConsumerDocumentController : AbpController, IConsumerDocumentAppService
{
    private readonly IConsumerDocumentAppService _consumerDocumentAppService;

    public ConsumerDocumentController(IConsumerDocumentAppService consumerDocumentAppService)
    {
        _consumerDocumentAppService = consumerDocumentAppService;
    }


    [HttpGet("{id}")]
    public async Task<ConsumerDocumentDto> GetAsync(Guid id)
    {
        return await _consumerDocumentAppService.GetAsync(id);
    }

    [HttpGet]
    public async Task<PagedResultDto<ConsumerDocumentDto>> GetListAsync(GetConsumerDocumentListDto input)
    {
        return await _consumerDocumentAppService.GetListAsync(input);
    }

    [HttpPost]
    public async Task<ConsumerDocumentDto> CreateAsync(CreateConsumerDocumentDto input)
    {
        return await _consumerDocumentAppService.CreateAsync(input);
    }

    [HttpPut("{id}")]
    public async Task UpdateAsync(Guid id, CreateConsumerDocumentDto input)
    {
        await _consumerDocumentAppService.UpdateAsync(id, input);
    }

    [HttpDelete("{id}")]
    public async Task DeleteAsync(Guid id)
    {
        await _consumerDocumentAppService.DeleteAsync(id);
    }
}
