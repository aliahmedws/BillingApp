using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc;

namespace Billing.ConsumerPersonalInfos;

[RemoteService(IsEnabled = true)]
[ControllerName("ConsumerPersonalInfos")]
[Area("app")]
[Route("api/app/consumer-personal-infos")]
public class ConsumerPersonalInfoController : AbpController, IConsumerPersonalInfoAppService
{
    private readonly IConsumerPersonalInfoAppService _consumerAppService;

    public ConsumerPersonalInfoController(IConsumerPersonalInfoAppService consumerAppService)
    {
        _consumerAppService = consumerAppService;
    }

    [HttpGet("{id}")]
    public async Task<ConsumerPersonalInfoDto> GetAsync(Guid id)
    {
        return await _consumerAppService.GetAsync(id);
    }

    [HttpGet]
    public async Task<PagedResultDto<ConsumerPersonalInfoDto>> GetListAsync(GetConsumerPersonalInfoListDto input)
    {
        return await _consumerAppService.GetListAsync(input);
    }

    [HttpPost]
    public async Task<ConsumerPersonalInfoDto> CreateAsync(CreateConsumerPersonalInfoDto input)
    {
        return await _consumerAppService.CreateAsync(input);
    }

    [HttpPut("{id}")]
    public async Task UpdateAsync(Guid id, UpdateConsumerPersonalInfoDto input)
    {
        await _consumerAppService.UpdateAsync(id, input);
    }

    [HttpDelete("{id}")]
    public async Task DeleteAsync(Guid id)
    {
        await _consumerAppService.DeleteAsync(id);
    }
}
