using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Billing.PlotInfos;

public interface IPlotInfoAppService : IApplicationService
{
    Task<PlotInfoDto> GetAsync(Guid id);

    Task<PagedResultDto<PlotInfoDto>> GetListAsync(GetPlotInfoListDto input);

    Task<PlotInfoDto> CreateAsync(CreatePlotInfoDto input);

    Task UpdateAsync(Guid id, UpdatePlotInfoDto input);

    Task DeleteAsync(Guid id);
    Task<List<PlotInfoLookupDto>> GetPlotLookUpAsync();
    Task<PlotInfoLookupDto?> GetPlotOwnerAsync(Guid plotId);

    Task<List<PlotInfoLookupDto?>> GetPlotsByBlockIdAsync(Guid blockId);
    Task<List<PlotInfoLookupDto>> GetPlotInfoByConsumerIdAsync(Guid consumerId);
}
