using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc;

namespace Billing.Phases;
[RemoteService(IsEnabled = true)]
[ControllerName("Phases")]
[Area("app")]
[Route("api/app/phases")]
public class PhaseController : AbpController, IPhaseAppService
{
    private readonly IPhaseAppService _phaseAppService;

    public PhaseController(IPhaseAppService phaseAppService)
    {
        _phaseAppService = phaseAppService;
    }

    [HttpGet("{id}")]
    public async Task<PhaseDto> GetAsync(Guid id)
    {
        return await _phaseAppService.GetAsync(id);
    }

    [HttpGet]
    public async Task<PagedResultDto<PhaseDto>> GetListAsync(GetPhaseListDto input)
    {
        return await _phaseAppService.GetListAsync(input);
    }

    [HttpPost]
    public async Task<PhaseDto> CreateAsync(CreatePhaseDto input)
    {
        return await _phaseAppService.CreateAsync(input);
    }

    [HttpPut("{id}")]
    public async Task UpdateAsync(Guid id, UpdatePhaseDto input)
    {
        await _phaseAppService.UpdateAsync(id, input);
    }

    [HttpDelete("{id}")]
    public async Task DeleteAsync(Guid id)
    {
        await _phaseAppService.DeleteAsync(id);
    }
}
