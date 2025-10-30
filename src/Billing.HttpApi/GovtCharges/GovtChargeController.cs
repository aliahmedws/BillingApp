using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc;

namespace Billing.GovtCharges;
[RemoteService(IsEnabled = true)]
[ControllerName("GovtCharges")]
[Area("app")]
[Route("api/app/govt-charges")]

public class GovtChargeController : AbpController 
{
    private readonly IGovtChargeAppService _govtChargeAppService;
    public GovtChargeController(IGovtChargeAppService govtChargeAppService)
    {
        _govtChargeAppService = govtChargeAppService;
    }

    [HttpGet("{id}")]
    public async Task<GovtChargeDto> GetAsync(Guid id)
    {
        return await _govtChargeAppService.GetAsync(id);
    }

    [HttpGet]
    public async Task<PagedResultDto<GovtChargeDto>> GetListAsync()
    {
        return await _govtChargeAppService.GetListAsync();
    }

    [HttpPut("{id}")]
    public async Task UpdateAsync(Guid id, UpdateGovtChargeDto input)
    {
        await _govtChargeAppService.UpdateAsync(id, input);
    }
}
