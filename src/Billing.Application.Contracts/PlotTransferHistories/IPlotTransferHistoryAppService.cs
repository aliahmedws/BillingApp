using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Billing.PlotTransferHistories;

public interface IPlotTransferHistoryAppService : IApplicationService
{
    Task<PlotTransferHistoryDto> GetAsync(Guid id);

    Task<PagedResultDto<PlotTransferHistoryDto>> GetListAsync(GetPlotTransferHistoryListDto input);

    Task<PlotTransferHistoryDto> CreateAsync(CreatePlotTransferHistoryDto input);

    Task UpdateAsync(Guid id, UpdatePlotTransferHistoryDto input);

    Task DeleteAsync(Guid id);

    Task ApproveAsync(Guid transferId);

    Task RejectAsync(Guid id, string? remarks = null);
}
