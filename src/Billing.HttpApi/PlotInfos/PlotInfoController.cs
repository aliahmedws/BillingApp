using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc;

namespace Billing.PlotInfos;

[RemoteService(IsEnabled = true)]
[ControllerName("PlotInfos")]
[Area("app")]
[Route("api/app/plot-infos")]
public class PlotInfoController : AbpController, IPlotInfoAppService
{
    private readonly IPlotInfoAppService _plotInfoAppService;

    public PlotInfoController(IPlotInfoAppService plotInfoAppService)
    {
        _plotInfoAppService = plotInfoAppService;
    }

    [HttpGet("{id}")]
    public async Task<PlotInfoDto> GetAsync(Guid id)
    {
        return await _plotInfoAppService.GetAsync(id);
    }

    [HttpGet]
    public async Task<PagedResultDto<PlotInfoDto>> GetListAsync(GetPlotInfoListDto input)
    {
        return await _plotInfoAppService.GetListAsync(input);
    }

    [HttpPost]
    public async Task<PlotInfoDto> CreateAsync(CreatePlotInfoDto input)
    {
        return await _plotInfoAppService.CreateAsync(input);
    }

    [HttpPut("{id}")]
    public async Task UpdateAsync(Guid id, UpdatePlotInfoDto input)
    {
        await _plotInfoAppService.UpdateAsync(id, input);
    }

    [HttpDelete("{id}")]
    public async Task DeleteAsync(Guid id)
    {
        await _plotInfoAppService.DeleteAsync(id);
    }

    [HttpGet("plotInfo-lookup")]
    public async Task<List<PlotInfoLookupDto>> GetPlotLookUpAsync()
    {
      return await _plotInfoAppService.GetPlotLookUpAsync();
    }

    [HttpGet("get-plot-owner/{plotId}")]
    public async Task<PlotInfoLookupDto?> GetPlotOwnerAsync(Guid plotId)
    {
        return await _plotInfoAppService.GetPlotOwnerAsync(plotId);
    }

    [HttpGet("by-block/{blockId}")]
    public async Task<List<PlotInfoLookupDto?>> GetPlotsByBlockIdAsync(Guid blockId)
    {
        return await _plotInfoAppService.GetPlotsByBlockIdAsync(blockId);
    }
}

