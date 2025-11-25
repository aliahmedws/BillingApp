using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace Billing.PlotInfos;

public interface IPlotInfoRepository : IRepository<PlotInfo, Guid>
{
    Task<PlotInfo?> FindByPlotNoAsync(string plotNo, Guid blockId);
    Task<PlotInfo?> FindByConsumerAsync(Guid consumerId, Guid blockId, Guid phaseId);
    Task<List<PlotInfo>> GetPlotLookUpAsync();
    Task<long> GetCountAsync(
        string? filter,
        string? plotNo,
        string? streetNo,
        Guid? blockId,
        Guid? phaseId,
        Guid? plotSizeId,
        PlotStatus? status,
        Guid? consumerId);
    Task<List<PlotInfo>> GetListAsync(
        int skipCount,
        int maxResultCount,
        string sorting,
        string? filter,
        string? plotNo,
        string? streetNo,
        Guid? blockId,
        Guid? phaseId,
        Guid? plotSizeId,
        PlotStatus? status,
        Guid? consumerId);
    Task ChangePlotOwnerAsync(Guid fromConsumerId, Guid toConsumerId, Guid plotId);
    Task<PlotInfo?> GetPlotOwnerAsync(Guid plotId);
    Task<List<PlotInfo>> GetPlotsByBlockIdAsync(Guid blockId);
    Task<PlotInfo?> GetPlotInfoByIdAsync(Guid id);
}
