using Asp.Versioning;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Content;

namespace Billing.MaintenancePaymentHistories;

[RemoteService(IsEnabled = true)]
[ControllerName("MaintenancePaymentHistory")]
[Area("app")]
[Route("api/app/maintenance-payment-history")]

public class MaintenancePaymentHistoryController : AbpController, IMaintenancePaymentHistoryAppService
{
    private readonly IMaintenancePaymentHistoryAppService _appService;

    public MaintenancePaymentHistoryController(
        IMaintenancePaymentHistoryAppService appService)
    {
        _appService = appService;
    }

    [HttpPost]
    public async Task<MaintenancePaymentHistoryDto> CreateAsync(CreateMaintenancePaymentHistoryDto input)
    {
        return await _appService.CreateAsync(input);
    }

    [HttpGet("{id}")]
    public async Task<MaintenancePaymentHistoryDto> GetAsync(Guid id)
    {
        return await _appService.GetAsync(id);
    }

    [HttpDelete("{id}")]
    public async Task DeleteAsync(Guid id)
    {
        await _appService.DeleteAsync(id);
    }

    [HttpGet]
    public async Task<PagedResultDto<MaintenancePaymentHistoryDto>> GetListAsync(GetMaintenancePaymentHistoryListDto input)
    {
        return await _appService.GetListAsync(input);
    }

    [HttpPut("update/{id}")]
    public Task UpdateAsync(Guid id, UpdateMaintenancePaymentHistoryDto input)
    {
        throw new NotImplementedException();
    }

    [HttpGet("download-template")]
    public Task<IRemoteStreamContent> DownloadImportTemplateAsync()
    {
        return _appService.DownloadImportTemplateAsync();
    }

    [ApiExplorerSettings(IgnoreApi = true)]
    [HttpPost("import")]
    public Task<ImportResultDto> ImportExcelFileAsync([FromForm] IFormFile file)
    {
        return _appService.ImportExcelFileAsync(file);
    }

}
