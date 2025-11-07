using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc;

namespace Billing.PlotTransferHistories;

[RemoteService(IsEnabled = true)]
[ControllerName("PlotTransferHistories")]
[Area("app")]
[Route("api/app/plot-transfer-histories")]
public class PlotTransferHistoryController : AbpController, IPlotTransferHistoryAppService
{
    private readonly IPlotTransferHistoryAppService _plotTransferHistoryAppService;

    public PlotTransferHistoryController(IPlotTransferHistoryAppService plotTransferHistoryAppService)
    {
        _plotTransferHistoryAppService = plotTransferHistoryAppService;
    }

    [HttpGet("{id}")]
    public async Task<PlotTransferHistoryDto> GetAsync(Guid id)
    {
        return await _plotTransferHistoryAppService.GetAsync(id);
    }

    [HttpGet]
    public async Task<PagedResultDto<PlotTransferHistoryDto>> GetListAsync(GetPlotTransferHistoryListDto input)
    {
        return await _plotTransferHistoryAppService.GetListAsync(input);
    }

    [HttpPost]
    public async Task<PlotTransferHistoryDto> CreateAsync(CreatePlotTransferHistoryDto input)
    {
        return await _plotTransferHistoryAppService.CreateAsync(input);
    }

    [HttpPut("{id}")]
    public async Task UpdateAsync(Guid id, UpdatePlotTransferHistoryDto input)
    {
        await _plotTransferHistoryAppService.UpdateAsync(id, input);
    }

    [HttpDelete("{id}")]
    public async Task DeleteAsync(Guid id)
    {
        await _plotTransferHistoryAppService.DeleteAsync(id);
    }

    [HttpPost("{transferId}/approve")]
    public async Task ApproveAsync(Guid transferId)
    {
        await _plotTransferHistoryAppService.ApproveAsync(transferId);
    }

    [HttpPost("{id}/reject")]
    public async Task RejectAsync(Guid id, string? remarks = null)
    {
        await _plotTransferHistoryAppService.RejectAsync(id, remarks);
    }
}
