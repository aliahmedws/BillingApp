using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc;

namespace Billing.MaintenanceBills;

[RemoteService(IsEnabled = true)]
[ControllerName("MaintenanceBills")]
[Area("app")]
[Route("api/app/maintenance-bills")]
public class MaintenanceBillController : AbpController, IMaintenanceBillAppService
{
    private readonly IMaintenanceBillAppService _maintenanceBillAppService;

    public MaintenanceBillController(IMaintenanceBillAppService maintenanceBillAppService)
    {
        _maintenanceBillAppService = maintenanceBillAppService;
    }

    [HttpGet("{id}")]
    public async Task<MaintenanceBillDto> GetAsync(Guid id)
    {
        return await _maintenanceBillAppService.GetAsync(id);
    }

    [HttpGet]
    public async Task<PagedResultDto<MaintenanceBillDto>> GetListAsync(GetMaintenanceBillListDto input)
    {
        return await _maintenanceBillAppService.GetListAsync(input);
    }

    [HttpPost]
    public async Task<MaintenanceBillDto> CreateAsync(CreateMaintenanceBillDto input)
    {
        return await _maintenanceBillAppService.CreateAsync(input);
    }

    [HttpPut("{id}")]
    public async Task UpdateAsync(Guid id, UpdateMaintenanceBillDto input)
    {
        await _maintenanceBillAppService.UpdateAsync(id, input);
    }

    [HttpDelete("{id}")]
    public async Task DeleteAsync(Guid id)
    {
        await _maintenanceBillAppService.DeleteAsync(id);
    }

    [HttpPost("generate")]
    public Task<GenerateMaintenanceBillsResultDto> GenerateAsync(GenerateMaintenanceBillsDto input)
    {
        return _maintenanceBillAppService.GenerateAsync(input);
    }
}

