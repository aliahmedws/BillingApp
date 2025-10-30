using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace Billing.PlotSizes;

public interface IPlotSizeRepository : IRepository<PlotSize, Guid>
{
    Task<List<PlotSize>> GetListAsync(
        int skipCount,
        int maxResultCount,
        string sorting,
        string? filter,
        string? SizeName,
        bool? isActive,
        PlotUnit? plotUnit);

    Task<long> GetCountAsync(
        string? filter,
        string? SizeName,
        bool? isActive,
        PlotUnit? plotUnit);
}
