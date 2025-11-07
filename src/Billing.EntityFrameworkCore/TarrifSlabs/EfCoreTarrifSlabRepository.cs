using Billing.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Billing.TarrifSlabs;

public class EfCoreTarrifSlabRepository : EfCoreRepository<BillingDbContext, TarrifSlab, Guid>, ITarrifSlabRepository
{
    public EfCoreTarrifSlabRepository(IDbContextProvider<BillingDbContext> dbContextProvider)  : base(dbContextProvider) { }

                //GET LIST ASYNC
    public async Task<List<TarrifSlab>> GetListAsync(
        int skipCount,
        int maxResultCount,
        string sorting,
        string? filter,
        decimal lowerSlab,
        decimal? upperSlab,
        decimal unitPrice
    )
    {
        var dbSet = await GetDbSetAsync();
        var query = dbSet.AsQueryable();

        query = query.OrderBy(sorting ?? "Id");

        return await query.Skip(skipCount).Take(maxResultCount).ToListAsync();
    }
                  //GET COUNT ASYNC
    public async Task<long> GetCountAsync(string? filter, decimal lowerSlab, decimal? upperSlab, decimal unitPrice)
    {
        var dbSet = await GetDbSetAsync();
        var query = dbSet.AsQueryable();

        return await query.LongCountAsync();
    }
}
