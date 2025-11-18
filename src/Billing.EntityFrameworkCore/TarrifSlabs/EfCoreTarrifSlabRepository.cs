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
    public async Task<TarrifSlab?> FindByExistance(decimal lowerSlab, decimal? upperSlab, decimal unitPrice)
    {
        var dbSet = await GetDbSetAsync();
        return await dbSet.FirstOrDefaultAsync(tarrifSlab => tarrifSlab.LowerSlab == lowerSlab &&
        tarrifSlab.UpperSlab == upperSlab && tarrifSlab.UnitPrice == unitPrice);
    }    
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
       var data = await ApplyFilterAsync(filter, lowerSlab, upperSlab, unitPrice);
        return await data.OrderBy(sorting).Skip(skipCount).Take(maxResultCount).ToListAsync();
    }

    public async Task<long> GetCountAsync(string? filter, decimal lowerSlab, decimal? upperSlab, decimal unitPrice)
    {
        var data = await ApplyFilterAsync(filter, lowerSlab, upperSlab, unitPrice);
        return await data.LongCountAsync();
    }

    private async Task<IQueryable<TarrifSlab>> ApplyFilterAsync(
         string? filter,
         decimal lowerSlab,
         decimal? upperSlab,
         decimal unitPrice)
    {
        var dbSet = await GetDbSetAsync();
        var query = dbSet.AsQueryable();

        if (lowerSlab > 0) query = query.Where(x => x.LowerSlab >= lowerSlab);
        if (upperSlab.HasValue) query = query.Where(x => x.UpperSlab <= upperSlab.Value);
        if (unitPrice > 0) query = query.Where(x => x.UnitPrice == unitPrice);

        return query;
    }
}