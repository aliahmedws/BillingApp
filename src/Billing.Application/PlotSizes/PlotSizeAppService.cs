using Billing.Permissions;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;

namespace Billing.PlotSizes;

[RemoteService(isEnabled: false)]
[Authorize(BillingPermissions.PlotSizes.Default)]
public class PlotSizeAppService : BillingAppService, IPlotSizeAppService
{
    private readonly IPlotSizeRepository _plotSizeRepository;
    private readonly PlotSizeManager _plotSizeManager;

    public PlotSizeAppService(IPlotSizeRepository plotSizeRepository, PlotSizeManager plotSizeManager)
    {
        _plotSizeRepository = plotSizeRepository;
        _plotSizeManager = plotSizeManager;
    }

    [Authorize(BillingPermissions.PlotSizes.Create)]
    public async Task<PlotSizeDto> CreateAsync(CreatePlotSizeDto input)
    {
        var plotSize = await _plotSizeManager.CreateAsync(
            input.SizeName,
            input.Area,
            input.Unit,
            input.Length,
            input.Width,
            input.Description,
            input.IsActive
        );

        await _plotSizeRepository.InsertAsync(plotSize);
        return ObjectMapper.Map<PlotSize, PlotSizeDto>(plotSize);
    }

    [Authorize(BillingPermissions.PlotSizes.Delete)]
    public async Task DeleteAsync(Guid id)
    {
        await _plotSizeRepository.DeleteAsync(id);
    }

    public async Task<PlotSizeDto> GetAsync(Guid id)
    {
        var plotSize = await _plotSizeRepository.GetAsync(id);
        return ObjectMapper.Map<PlotSize, PlotSizeDto>(plotSize);
    }

    public async Task<PagedResultDto<PlotSizeDto>> GetListAsync(GetPlotSizeListDto input)
    {
        if (input.Sorting.IsNullOrWhiteSpace())
        {
            input.Sorting = nameof(PlotSize.CreationTime);
        }

        var plotSizes = await _plotSizeRepository.GetListAsync(
            input.SkipCount,
            input.MaxResultCount,
            input.Sorting,
            input.Filter,
            input.SizeName,
            input.IsActive,
            input.Unit
        );

        var totalCount = await _plotSizeRepository.GetCountAsync(
            input.Filter,
            input.SizeName,
            input.IsActive,
            input.Unit
        );

        return new PagedResultDto<PlotSizeDto>(
            totalCount,
            ObjectMapper.Map<List<PlotSize>, List<PlotSizeDto>>(plotSizes)
        );
    }

    [Authorize(BillingPermissions.PlotSizes.Edit)]
    public async Task UpdateAsync(Guid id, UpdatePlotSizeDto input)
    {
        var plotSize = await _plotSizeRepository.GetAsync(id);

        var updated = await _plotSizeManager.UpdateAsync(
            plotSize,
            input.SizeName,
            input.Area,
            input.Unit,
            input.Length,
            input.Width,
            input.Description,
            input.IsActive
        );

        await _plotSizeRepository.UpdateAsync(updated);
    }

    public async Task<List<PlotSizeLookupDto>> GetPlotSizeLookupAsync()
    {
        var data = await _plotSizeRepository.GetPlotSizeLookupAsync();
        var query = data.Select(x => new PlotSizeLookupDto
        {
            Id = x.Id,
            PlotSizeName = x.SizeName
        }).ToList();

        return query;
    }
}