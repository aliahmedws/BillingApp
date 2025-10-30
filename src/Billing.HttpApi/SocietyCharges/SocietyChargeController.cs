using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc;

namespace Billing.SocietyCharges;
[RemoteService(IsEnabled = true)]
[ControllerName("SocietyCharges")]
[Area("app")]
[Route("api/app/society-charges")]

public class SocietyChargeController : AbpController, ISocietyChargeAppService
{
    private readonly ISocietyChargeAppService _societyChargeAppService;
    public SocietyChargeController(ISocietyChargeAppService societyChargeAppService)
    {
        _societyChargeAppService = societyChargeAppService;
    }

    [HttpGet("{id}")]
    public async Task<SocietyChargeDto> GetAsync(Guid id)
    {
        return await _societyChargeAppService.GetAsync(id);
    }

    [HttpGet]
    public async Task<PagedResultDto<SocietyChargeDto>> GetListAsync(GetSocietyChargeListDto input)
    {
        return await _societyChargeAppService.GetListAsync(input);
    }

    [HttpPost]
    public async Task<SocietyChargeDto> CreateAsync(CreateSocietyChargeDto input)
    {
        return await _societyChargeAppService.CreateAsync(input);
    }

    [HttpPut("{id}")]
    public async Task UpdateAsync(Guid id, UpdateSocietyChargeDto input)
    {
        await _societyChargeAppService.UpdateAsync(id, input);
    }

    [HttpDelete("{id}")]
    public async Task DeleteAsync(Guid id)
    {
        await _societyChargeAppService.DeleteAsync(id);
    }
}
