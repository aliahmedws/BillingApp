using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Content;

namespace Billing.ElectricityBills;

[RemoteService(IsEnabled = true)]
[ControllerName("ElectricityBill")]
[Area("app")]
[Route("api/app/electricity-bills")]
public class ElectricityBillController : AbpController, IElectricityBillAppService
{
    private readonly IElectricityBillAppService _electricityBillAppService;

    public ElectricityBillController(IElectricityBillAppService electricityBillAppService)
    {
        _electricityBillAppService = electricityBillAppService;
    }

    [HttpGet("{id}")]
    public async Task<ElectricityBillDto> GetAsync(Guid id)
    {
        return await _electricityBillAppService.GetAsync(id);
    }

    [HttpGet]
    public async Task<PagedResultDto<ElectricityBillDto>> GetListAsync(GetElectricityBillListDto input)
    {
        return await _electricityBillAppService.GetListAsync(input);
    }

    [HttpPost]
    public async Task<ElectricityBillDto> CreateAsync(CreateElectricityBillDto input)
    {
        return await _electricityBillAppService.CreateAsync(input);
    }

    [HttpPut("{id}")]
    public async Task UpdateAsync(Guid id, UpdateElectricityBillDto input)
    {
        await _electricityBillAppService.UpdateAsync(id, input);
    }

    [HttpDelete("{id}")]
    public async Task DeleteAsync(Guid id)
    {
        await _electricityBillAppService.DeleteAsync(id);
    }

    [HttpGet("calculate")]
    public Task<decimal> CalculateBillAsync(int units)
    {
        return _electricityBillAppService.CalculateBillAsync(units);
    }

    [HttpPost("generate-bulk")]
    public async Task GenerateBulkAsync(BulkElectricityBillRequestDto input)
    {
        await _electricityBillAppService.GenerateBulkAsync(input);
    }

    [HttpGet("export-excel")]
    public Task<IRemoteStreamContent> GetListAsExcelFileAsync(GetElectricityBillListDto input)
    {
       return _electricityBillAppService.GetListAsExcelFileAsync(input);
    }
}
