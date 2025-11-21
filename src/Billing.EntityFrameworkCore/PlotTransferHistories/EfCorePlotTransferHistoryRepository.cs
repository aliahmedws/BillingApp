using Billing.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Billing.PlotTransferHistories;

public class EfCorePlotTransferHistoryRepository : EfCoreRepository<BillingDbContext, PlotTransferHistory, Guid>, IPlotTransferHistoryRepository
{
    public EfCorePlotTransferHistoryRepository(IDbContextProvider<BillingDbContext> dbContextProvider) : base(dbContextProvider)
    {
    }

    public async Task<PlotTransferHistory?> FindByRegistryNoAsync(string registryNo)
    {
        var dbSet = await GetDbSetAsync();
        return await dbSet.FirstOrDefaultAsync(x => x.RegistryNo == registryNo);
    }

    public async Task<List<PlotTransferHistory>> GetListAsync(
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
        TransferStatus? status)
    {
        var query = await GetFilteredQueryAsync(
            filter, plotId, fromConsumerId, toConsumerId, transferDate,
            transferType, registryNo, approvedByUserId, approvedAt, status);

        return await query
            .OrderBy(sorting)
            .PageBy(skipCount, maxResultCount)
            .ToListAsync();
    }

    public async Task<long> GetCountAsync(
        string? filter,
        Guid? plotId,
        Guid? fromConsumerId,
        Guid? toConsumerId,
        DateTime? transferDate,
        TransferType? transferType,
        string? registryNo,
        Guid? approvedByUserId,
        DateTime? approvedAt,
        TransferStatus? status)
    {
        var query = await GetFilteredQueryAsync(
            filter, plotId, fromConsumerId, toConsumerId, transferDate,
            transferType, registryNo, approvedByUserId, approvedAt, status);

        return await query.LongCountAsync();
    }

    private async Task<IQueryable<PlotTransferHistory>> GetFilteredQueryAsync(
        string? filter,
        Guid? plotId,
        Guid? fromConsumerId,
        Guid? toConsumerId,
        DateTime? transferDate,
        TransferType? transferType,
        string? registryNo,
        Guid? approvedByUserId,
        DateTime? approvedAt,
        TransferStatus? status)
    {
        var queryable = await GetQueryableAsync();

        var query = queryable
            .Include(x => x.Plot)
            .Include(x => x.Consumers)
            .Include(x => x.FromConsumers)
            .Include(x => x.ApprovedByUser)
            .WhereIf(!filter.IsNullOrWhiteSpace(),
                x => x.RegistryNo.ToLower().Contains(filter!.ToLower())
                  || x.Remarks!.ToLower().Contains(filter.ToLower()))
            .WhereIf(plotId.HasValue, x => x.PlotId == plotId)
            .WhereIf(fromConsumerId.HasValue, x => x.FromConsumerId == fromConsumerId)
            .WhereIf(toConsumerId.HasValue, x => x.ToConsumerId == toConsumerId)
            .WhereIf(transferDate.HasValue, x => x.TransferDate.Date == transferDate!.Value.Date)
            .WhereIf(transferType.HasValue, x => x.TransferType == transferType)
            .WhereIf(!registryNo.IsNullOrWhiteSpace(), x => x.RegistryNo.ToLower().Contains(registryNo!.ToLower()))
            .WhereIf(approvedByUserId.HasValue, x => x.ApprovedByUserId == approvedByUserId)
            .WhereIf(approvedAt.HasValue, x => x.ApprovedAt!.Value.Date == approvedAt!.Value.Date)
            .WhereIf(status.HasValue, x => x.Status == status);

        return query;
    }
}
