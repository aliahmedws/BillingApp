using Billing.Permissions;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;

namespace Billing.PlotTransferHistories;

[RemoteService(IsEnabled = false)]
[Authorize(BillingPermissions.PlotTransferHistories.Default)]
public class PlotTransferHistoryAppService : BillingAppService, IPlotTransferHistoryAppService
{
    private readonly IPlotTransferHistoryRepository _plotTransferHistoryRepository;
    private readonly PlotTransferHistoryManager _plotTransferHistoryManager;

    public PlotTransferHistoryAppService(
        IPlotTransferHistoryRepository plotTransferHistoryRepository,
        PlotTransferHistoryManager plotTransferHistoryManager)
    {
        _plotTransferHistoryRepository = plotTransferHistoryRepository;
        _plotTransferHistoryManager = plotTransferHistoryManager;
    }

    [Authorize(BillingPermissions.PlotTransferHistories.Create)]
    public async Task<PlotTransferHistoryDto> CreateAsync(CreatePlotTransferHistoryDto input)
    {
        var history = await _plotTransferHistoryManager.CreateAsync(
            input.PlotId,
            input.FromConsumerId,
            input.ToConsumerId,
            input.TransferDate,
            input.TransferType,
            input.RegistryNo,
            input.ConsiderationAmount,
            input.Remarks
        );

        await _plotTransferHistoryRepository.InsertAsync(history);
        return ObjectMapper.Map<PlotTransferHistory, PlotTransferHistoryDto>(history);
    }

    [Authorize(BillingPermissions.PlotTransferHistories.Delete)]
    public async Task DeleteAsync(Guid id)
    {
        await _plotTransferHistoryRepository.DeleteAsync(id);
    }

    public async Task<PlotTransferHistoryDto> GetAsync(Guid id)
    {
        var history = await _plotTransferHistoryRepository.GetAsync(id);
        return ObjectMapper.Map<PlotTransferHistory, PlotTransferHistoryDto>(history);
    }

    public async Task<PagedResultDto<PlotTransferHistoryDto>> GetListAsync(GetPlotTransferHistoryListDto input)
    {
        if (input.Sorting.IsNullOrWhiteSpace())
        {
            input.Sorting = nameof(PlotTransferHistory.TransferDate);
        }

        var items = await _plotTransferHistoryRepository.GetListAsync(
            input.SkipCount,
            input.MaxResultCount,
            input.Sorting!,
            input.Filter,
            input.PlotId,
            input.FromConsumerId,
            input.ToConsumerId,
            input.TransferDate,
            input.TransferType,
            input.RegistryNo,
            input.ApprovedByUserId,
            input.ApprovedAt,
            input.IsApproved
        );

        var totalCount = await _plotTransferHistoryRepository.GetCountAsync(
            input.Filter,
            input.PlotId,
            input.FromConsumerId,
            input.ToConsumerId,
            input.TransferDate,
            input.TransferType,
            input.RegistryNo,
            input.ApprovedByUserId,
            input.ApprovedAt,
            input.IsApproved
        );

        var dtos = ObjectMapper.Map<List<PlotTransferHistory>, List<PlotTransferHistoryDto>>(items);
        return new PagedResultDto<PlotTransferHistoryDto>(totalCount, dtos);
    }

    [Authorize(BillingPermissions.PlotTransferHistories.Edit)]
    public async Task UpdateAsync(Guid id, UpdatePlotTransferHistoryDto input)
    {
        var history = await _plotTransferHistoryRepository.GetAsync(id);

        await _plotTransferHistoryManager.UpdateAsync(
            history,
            input.PlotId,
            input.FromConsumerId,
            input.ToConsumerId,
            input.TransferDate,
            input.TransferType,
            input.RegistryNo,
            input.ConsiderationAmount,
            input.Remarks
        );

        await _plotTransferHistoryRepository.UpdateAsync(history);
    }

    [Authorize(BillingPermissions.PlotTransferHistories.Approved)]
    public async Task ApproveAsync(Guid transferId)
    {
        await _plotTransferHistoryManager.ApproveAsync(transferId);
    }

    [Authorize(BillingPermissions.PlotTransferHistories.Reject)]
    public async Task RejectAsync(Guid id, string? remarks = null)
    {
        await _plotTransferHistoryManager.RejectAsync(id, remarks);
    }
}
