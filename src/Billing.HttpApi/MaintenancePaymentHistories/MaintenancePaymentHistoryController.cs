using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc;

namespace Billing.MaintenancePaymentHistories;

[RemoteService(IsEnabled = true)]
[ControllerName("MaintenancePaymentHistories")]
[Area("app")]
[Route("api/app/maintenance-payment-histories")]
public class MaintenancePaymentHistoryController : AbpController, IMaintenancePaymentHistoryAppService
{
    private readonly IMaintenancePaymentHistoryAppService _maintenancePaymentHistoryAppService;
    public MaintenancePaymentHistoryController(IMaintenancePaymentHistoryAppService maintenancePaymentHistoryAppService)
    {
        _maintenancePaymentHistoryAppService = maintenancePaymentHistoryAppService;
    }

    [HttpGet("{id}")]
    public async Task<MaintenancePaymentHistoryDto> GetAsync(Guid id)
    {
        return await _maintenancePaymentHistoryAppService.GetAsync(id);
    }

    [HttpGet]
    public async Task<PagedResultDto<MaintenancePaymentHistoryDto>> GetListAsync(GetMaintenancePaymentHistoryListDto input)
    {
        return await _maintenancePaymentHistoryAppService.GetListAsync(input);
    }

    [HttpPost]
    public async Task<MaintenancePaymentHistoryDto> CreateAsync(CreateMaintenancePaymentHistoryDto input)
    {
        return await _maintenancePaymentHistoryAppService.CreateAsync(input);
    }

    [HttpPut("{id}")]
    public async Task UpdateAsync(Guid id, UpdateMaintenancePaymentHistoryDto input)
    {
        await _maintenancePaymentHistoryAppService.UpdateAsync(id, input);
    }

    [HttpDelete("{id}")]
    public async Task DeleteAsync(Guid id)
    {
        await _maintenancePaymentHistoryAppService.DeleteAsync(id);
    }
}
