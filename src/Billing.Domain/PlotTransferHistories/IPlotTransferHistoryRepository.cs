using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace Billing.PlotTransferHistories;

public interface IPlotTransferHistoryRepository : IRepository<PlotTransferHistory, Guid>
{
    Task<PlotTransferHistory?> FindByRegistryNoAsync(string registryNo);
    Task<List<PlotTransferHistory>> GetListAsync(
        int skipCount,
        int maxResultCount,
        string sorting,
        string? filter,
        Guid? plotId,
        Guid? fromConsumerId,
        Guid? toConsumerId,
        DateTime? transferDate,
        TransferType? transferType,
        string? registryNo,
        Guid? approvedByUserId,
        DateTime? approvedAt,
        bool? isApproved);
    Task<long> GetCountAsync(
        string? filter,
        Guid? plotId,
        Guid? fromConsumerId,
        Guid? toConsumerId,
        DateTime? transferDate,
        TransferType? transferType,
        string? registryNo,
        Guid? approvedByUserId,
        DateTime? approvedAt,
        bool? isApproved);
}
