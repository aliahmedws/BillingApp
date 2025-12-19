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
    public EfCoreTarrifSlabRepository(IDbContextProvider<BillingDbContext> dbContextProvider) : base(dbContextProvider) { }
    public async Task<TarrifSlab> FindByLowerSlab(decimal lowerSlab)
    {
        var dbSet = await GetDbSetAsync();
        return await dbSet.FirstOrDefaultAsync(tarrifSlab => tarrifSlab.LowerSlab == lowerSlab);
    }

    public async Task<TarrifSlab?> FindByUpperSlab(decimal? upperSlab)
    {
        if (!upperSlab.HasValue) return null;

        var dbSet = await GetDbSetAsync();
        return await dbSet.FirstOrDefaultAsync(tarrifSlab => tarrifSlab.UpperSlab == upperSlab.Value);
    }

    public async Task<TarrifSlab> FindByUnitPrice(decimal unitPrice)
    {
        var dbSet = await GetDbSetAsync();
        return await dbSet.FirstOrDefaultAsync(tarrifSlab => tarrifSlab.UnitPrice == unitPrice);
    }

    public async Task<List<TarrifSlab>> GetListAsync(
        int skipCount,
        int maxResultCount,
        string sorting,
        string? filter,
        decimal? lowerSlab,
        decimal? upperSlab,
        decimal? unitPrice
    )
    {
        var data = await ApplyFilterAsync(filter, lowerSlab, upperSlab, unitPrice);

        return await data
            .OrderBy(sorting)
            //.PageBy(skipCount, maxResultCount)
            .Skip(skipCount)
            .Take(maxResultCount)
            .ToListAsync();
    }

    public async Task<long> GetCountAsync(string? filter, decimal? lowerSlab, decimal? upperSlab, decimal? unitPrice)
    {
        var data = await ApplyFilterAsync(filter, lowerSlab, upperSlab, unitPrice);
        return await data.LongCountAsync();
    }

    private async Task<IQueryable<TarrifSlab>> ApplyFilterAsync(
         string? filter,
         decimal? lowerSlab,
         decimal? upperSlab,
         decimal? unitPrice)
    {
        var dbSet = await GetDbSetAsync();
        var query = dbSet.Include(x => x.Creator).Include(x => x.LastModifier).AsQueryable();

        if (lowerSlab > 0) query = query.Where(x => x.LowerSlab >= lowerSlab);
        if (upperSlab.HasValue) query = query.Where(x => x.UpperSlab <= upperSlab.Value);
        if (unitPrice > 0) query = query.Where(x => x.UnitPrice == unitPrice);

        if (!string.IsNullOrWhiteSpace(filter))
        {
            query = query.Where(x =>
              x.LowerSlab.ToString().Contains(filter!) ||
              (x.UpperSlab != null && x.UpperSlab.Value.ToString().Contains(filter!)) ||
              x.UnitPrice.ToString().Contains(filter!));

        }
        return query;
    }
}