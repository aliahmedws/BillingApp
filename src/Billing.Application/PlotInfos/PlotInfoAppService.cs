using Billing.Permissions;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;

namespace Billing.PlotInfos;

[RemoteService(isEnabled: false)]
[Authorize(BillingPermissions.PlotInfos.Default)]
public class PlotInfoAppService : BillingAppService, IPlotInfoAppService
{
    private readonly IPlotInfoRepository _plotInfoRepository;
    private readonly PlotInfoManager _plotInfoManager;

    public PlotInfoAppService(
        IPlotInfoRepository plotInfoRepository,
        PlotInfoManager plotInfoManager)
    {
        _plotInfoRepository = plotInfoRepository;
        _plotInfoManager = plotInfoManager;
    }

    [Authorize(BillingPermissions.PlotInfos.Create)]
    public async Task<PlotInfoDto> CreateAsync(CreatePlotInfoDto input)
    {
        var plot = await _plotInfoManager.CreateAsync(
            input.PlotNo,
            input.PlotType,
            input.StreetNo,
            input.PlotSizeId,
            input.Status,
            input.BlockId,
            input.ConsumerId,
            input.PhaseId,
            input.Remarks
        );

        await _plotInfoRepository.InsertAsync(plot, autoSave: true);

        return ObjectMapper.Map<PlotInfo, PlotInfoDto>(plot);
    }

    [Authorize(BillingPermissions.PlotInfos.Edit)]
    public async Task UpdateAsync(Guid id, UpdatePlotInfoDto input)
    {
        var plot = await _plotInfoRepository.GetAsync(id);

        await _plotInfoManager.UpdateAsync(
            plot,
            input.PlotNo,
            input.PlotType,
            input.StreetNo,
            input.PlotSizeId,
            input.Status,
            input.BlockId,
            input.ConsumerId,
            input.PhaseId,
            input.Remarks
        );

        await _plotInfoRepository.UpdateAsync(plot);
    }

    [Authorize(BillingPermissions.PlotInfos.Delete)]
    public async Task DeleteAsync(Guid id)
    {
        await _plotInfoRepository.DeleteAsync(id);
    }

    public async Task<PlotInfoDto> GetAsync(Guid id)
    {
        var plot = await _plotInfoRepository.GetPlotInfoByIdAsync(id);
        return ObjectMapper.Map<PlotInfo, PlotInfoDto>(plot!);
    }

    public async Task<PagedResultDto<PlotInfoDto>> GetListAsync(GetPlotInfoListDto input)
    {
        if (input.Sorting.IsNullOrWhiteSpace())
        {
            input.Sorting = nameof(PlotInfo.PlotNo);
        }

        var items = await _plotInfoRepository.GetListAsync(
            input.SkipCount,
            input.MaxResultCount,
            input.Sorting!,
            input.Filter,
            input.PlotNo,
            input.StreetNo,
            input.BlockId,
            input.PhaseId,
            input.PlotSizeId,
            input.Status,
            input.ConsumerId
        );

        var totalCount = await _plotInfoRepository.GetCountAsync(
            input.Filter,
            input.PlotNo,
            input.StreetNo,
            input.BlockId,
            input.PhaseId,
            input.PlotSizeId,
            input.Status,
            input.ConsumerId
        );

        return new PagedResultDto<PlotInfoDto>(
            totalCount,
            ObjectMapper.Map<List<PlotInfo>, List<PlotInfoDto>>(items)
        );
    }

    public async Task<List<PlotInfoLookupDto>> GetPlotLookUpAsync()
    {
        var data = await _plotInfoRepository.GetPlotLookUpAsync();
        return data.Select(x => new PlotInfoLookupDto
        {
            Id = x.Id,
            PlotNo = x.PlotNo
        }).ToList();
    }

    public async Task<PlotInfoLookupDto?> GetPlotOwnerAsync(Guid plotId)
    {
        var data = await _plotInfoRepository.GetPlotOwnerAsync(plotId);

        if (data?.ConsumerPersonaInfo == null)
            return null;

        var result = new PlotInfoLookupDto
        {
            Id = data!.Id,
            ConsumerId = data.ConsumerPersonaInfo.Id,
            ConsumerName = data.ConsumerPersonaInfo.FirstName + " " + data.ConsumerPersonaInfo.LastName
        };

        return result;
    }

    public async Task<List<PlotInfoLookupDto?>> GetPlotsByBlockIdAsync(Guid blockId)
    {
        var data = await _plotInfoRepository.GetPlotsByBlockIdAsync(blockId);

        var result = data.Select(x => new PlotInfoLookupDto
        {
            Id = x.Id,
            PlotNo = x.PlotNo,
            ConsumerName = "",
            ConsumerId = Guid.Empty
        }).ToList();

        return result!;
    }
}

