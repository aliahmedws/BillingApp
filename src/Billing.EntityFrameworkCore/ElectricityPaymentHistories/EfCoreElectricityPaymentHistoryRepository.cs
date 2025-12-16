using Billing.EntityFrameworkCore;
using Billing.MaintenancePaymentHistories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Billing.ElectricityPaymentHistories;

public class EfCoreElectricityPaymentHistoryRepository : EfCoreRepository<BillingDbContext,
    ElectricityPaymentHistory, Guid>, IElectricityPaymentHistoryRepository
{
    public EfCoreElectricityPaymentHistoryRepository(IDbContextProvider<BillingDbContext> dbContextProvider)
         : base(dbContextProvider)
    {
    }

    public async Task<ElectricityPaymentHistory?> FindByTransactionIdAsync(string transactionId)
    {
        var query = await GetDbSetAsync();
        return await query.FirstOrDefaultAsync(x => x.TransactionId == transactionId);
    }

    public async Task<List<ElectricityPaymentHistory>> GetListAsync(
          int skipCount,
          int maxResultCount,
          string sorting,
          string? filter = null,
          string? transactionId = null,
          PaymentMethod? method = null,
          Guid? electricityBillId = null
      )
    {
        var query = await GetFilteredQueryableAsync(filter, electricityBillId, transactionId, method);

        return await query
            .OrderBy(sorting)
            .Skip(skipCount)
            .Take(maxResultCount)
            .ToListAsync();
    }

    public async Task<long> GetCountAsync(
          string? filter = null,
          string? transactionId = null,
          PaymentMethod? method = null,
          Guid? electricityBillId = null
    )
    {
        var query = await GetFilteredQueryableAsync(filter, electricityBillId, transactionId, method);


        return await query.LongCountAsync();
    }

    public async Task<ElectricityPaymentHistory?> GetLatestPaymentHistoryByBillIdAsync(Guid electricityBillId)
    {
        var dbSet = await GetDbSetAsync();

        return await dbSet
            .Where(x => x.ElectricityBillId == electricityBillId)
            .OrderByDescending(x => x.CreationTime)
            .FirstOrDefaultAsync();
    }


    private async Task<IQueryable<ElectricityPaymentHistory>> GetFilteredQueryableAsync(
         string? filter,
         Guid? electricityBillId,
         string? transactionId,
         PaymentMethod? method
     )
    {
        var queryable = await GetQueryableAsync();

        var query = queryable
            .WhereIf(electricityBillId.HasValue, x => x.ElectricityBillId == electricityBillId)
            .WhereIf(method.HasValue, x => x.Method == method)
            .WhereIf(!string.IsNullOrWhiteSpace(transactionId), x => x.TransactionId == transactionId)
            .WhereIf(!string.IsNullOrWhiteSpace(filter),
            x => x.TransactionId.Contains(filter!) || x.Method.ToString().Contains(filter!));

        return query;
    }
}
