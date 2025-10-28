using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Billing.PlotSizes;

public interface IPlotSizeAppService : IApplicationService
{
    Task<PlotSizeDto> GetAsync(Guid id);

    Task<PagedResultDto<PlotSizeDto>> GetListAsync(GetPlotSizeListDto input);

    Task<PlotSizeDto> CreateAsync(CreatePlotSizeDto input);

    Task UpdateAsync(Guid id, UpdatePlotSizeDto input);

    Task DeleteAsync(Guid id);
}
