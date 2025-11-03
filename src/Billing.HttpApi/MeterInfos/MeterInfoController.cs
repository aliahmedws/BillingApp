using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc;

namespace Billing.MeterInfos;

[RemoteService(IsEnabled = true)]
[ControllerName("MeterInfos")]
[Area("app")]
[Route("api/app/meter-infos")]
public class MeterInfoController : AbpController, IMeterInfoAppService
{
    private readonly IMeterInfoAppService _meterInfoAppService;

    public MeterInfoController(IMeterInfoAppService meterInfoAppService)
    {
        _meterInfoAppService = meterInfoAppService;
    }

    [HttpGet("{id}")]
    public async Task<MeterInfoDto> GetAsync(Guid id)
    {
        return await _meterInfoAppService.GetAsync(id);
    }

    [HttpGet]
    public async Task<PagedResultDto<MeterInfoDto>> GetListAsync(GetMeterInfoListDto input)
    {
        return await _meterInfoAppService.GetListAsync(input);
    }

    [HttpPost]
    public async Task<MeterInfoDto> CreateAsync(CreateMeterInfoDto input)
    {
        return await _meterInfoAppService.CreateAsync(input);
    }

    [HttpPut("{id}")]
    public async Task UpdateAsync(Guid id, UpdateMeterInfoDto input)
    {
        await _meterInfoAppService.UpdateAsync(id, input);
    }

    [HttpDelete("{id}")]
    public async Task DeleteAsync(Guid id)
    {
        await _meterInfoAppService.DeleteAsync(id);
    }
}
