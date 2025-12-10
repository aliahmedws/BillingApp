using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc;

namespace Billing.ElectricityPaymentHistories;

[RemoteService(IsEnabled = true)]
[ControllerName("ElectricityPaymentHistories")]
[Area("app")]
[Route("api/app/electricity-payment-histories")]
public class ElectricityPaymentHistoryController : AbpController, IElectricityPaymentHistoryAppService
{
    private readonly IElectricityPaymentHistoryAppService _electricityPaymentHistoryAppService;
    public ElectricityPaymentHistoryController(IElectricityPaymentHistoryAppService electricityPaymentHistoryAppService)
    {
        _electricityPaymentHistoryAppService = electricityPaymentHistoryAppService;
    }

    [HttpGet("{id}")]
    public async Task<ElectricityPaymentHistoryDto> GetAsync(Guid id)
    {
        return await _electricityPaymentHistoryAppService.GetAsync(id);
    }

    [HttpGet]
    public async Task<PagedResultDto<ElectricityPaymentHistoryDto>> GetListAsync(GetElectricityPaymentHistoryListDto input)
    {
        return await _electricityPaymentHistoryAppService.GetListAsync(input);
    }

    [HttpPost]
    public async Task<ElectricityPaymentHistoryDto> CreateAsync(CreateElectricityPaymentHistoryDto input)
    {
        return await _electricityPaymentHistoryAppService.CreateAsync(input);
    }

    [HttpPut("{id}")]
    public async Task UpdateAsync(Guid id, UpdateElectricityPaymentHistoryDto input)
    {
        await _electricityPaymentHistoryAppService.UpdateAsync(id, input);
    }

    [HttpDelete("{id}")]
    public async Task DeleteAsync(Guid id)
    {
        await _electricityPaymentHistoryAppService.DeleteAsync(id);
    }
}
