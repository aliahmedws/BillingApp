using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc;

namespace Billing.IescoCharges;
[RemoteService(IsEnabled = true)]
[ControllerName("IescoCharges")]
[Area("app")]
[Route("api/app/iesco-charges")]
public class IescoChargeController: AbpController
{
    private readonly IIescoChargeAppService _iescoChargeAppService;
    public IescoChargeController(IIescoChargeAppService iescoChargeAppService)
    {
        _iescoChargeAppService = iescoChargeAppService;
    }

    [HttpGet("{id}")]
    public async Task<IescoChargeDto> GetAsync(Guid id)
    {
        return await _iescoChargeAppService.GetAsync(id);
    }

    [HttpGet]
    public async Task<PagedResultDto<IescoChargeDto>> GetListAsync()
    {
        return await _iescoChargeAppService.GetListAsync();
    }

    [HttpPut("{id}")]
    public async Task UpdateAsync(Guid id, UpdateIescoChargeDto input)
    {
        await _iescoChargeAppService.UpdateAsync(id, input);
    }
}
