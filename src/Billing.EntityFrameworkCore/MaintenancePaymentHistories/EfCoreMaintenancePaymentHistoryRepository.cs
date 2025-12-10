using Billing.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Billing.MaintenancePaymentHistories;

public class EfCoreMaintenancePaymentHistoryRepository : EfCoreRepository<BillingDbContext,
    MaintenancePaymentHistory, Guid>, IMaintenancePaymentHistoryRepository
{
    public EfCoreMaintenancePaymentHistoryRepository(IDbContextProvider<BillingDbContext> dbContextProvider)
         : base(dbContextProvider)
    {
    }

    public async Task<MaintenancePaymentHistory?> FindByTransactionIdAsync(string transactionId)
    {
        var dbSet = await GetDbSetAsync();
        return await dbSet.FirstOrDefaultAsync(x => x.TransactionId == transactionId);
    }

    public async Task<List<MaintenancePaymentHistory>> GetListAsync(
          int skipCount,
          int maxResultCount,
          string sorting,
          Guid? maintenanceBillId = null,
          string? filter = null
      )
    {
        var query = await GetFilterAsync(filter, maintenanceBillId);

        var result = query
            .OrderBy(sorting)
            .Skip(skipCount)
            .Take(maxResultCount);

        return await result.ToListAsync();
    }

    public async Task<long> GetCountAsync(
         string? filter,
         PaymentMethod? method,
         DateTime? paymentDate,
         Guid? maintenanceBillId
    )
    {
        var query = await GetFilterAsync(filter, maintenanceBillId);

        return await query.LongCountAsync();
    }

    private async Task<IQueryable<MaintenancePaymentHistory>> GetFilterAsync(
         string? filter = null,
         Guid? maintenanceBillId = null,
         string? transactionId = null
     )
    {
        var queryable = await GetQueryableAsync();

        var query = queryable
            .WhereIf(maintenanceBillId.HasValue, x => x.MaintenanceBillId == maintenanceBillId)
            .WhereIf(!string.IsNullOrWhiteSpace(transactionId), x => x.TransactionId == transactionId)
            .WhereIf(!string.IsNullOrWhiteSpace(filter),
            x => x.TransactionId.Contains(filter!) || x.Method.ToString().Contains(filter!));

        return query;
    }

}
