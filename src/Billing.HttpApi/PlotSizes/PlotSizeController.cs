using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc;

namespace Billing.PlotSizes;

[RemoteService(IsEnabled = true)]
[ControllerName("PlotSizes")]
[Area("app")]
[Route("api/app/plot-sizes")]
public class PlotSizeController : AbpController, IPlotSizeAppService
{
    private readonly IPlotSizeAppService _plotSizeAppService;

    public PlotSizeController(IPlotSizeAppService plotSizeAppService)
    {
        _plotSizeAppService = plotSizeAppService;
    }

    [HttpGet("{id}")]
    public async Task<PlotSizeDto> GetAsync(Guid id)
    {
        return await _plotSizeAppService.GetAsync(id);
    }

    [HttpGet]
    public async Task<PagedResultDto<PlotSizeDto>> GetListAsync(GetPlotSizeListDto input)
    {
        return await _plotSizeAppService.GetListAsync(input);
    }

    [HttpPost]
    public async Task<PlotSizeDto> CreateAsync(CreatePlotSizeDto input)
    {
        return await _plotSizeAppService.CreateAsync(input);
    }

    [HttpPut("{id}")]
    public async Task UpdateAsync(Guid id, UpdatePlotSizeDto input)
    {
        await _plotSizeAppService.UpdateAsync(id, input);
    }

    [HttpDelete("{id}")]
    public async Task DeleteAsync(Guid id)
    {
        await _plotSizeAppService.DeleteAsync(id);
    }
}
