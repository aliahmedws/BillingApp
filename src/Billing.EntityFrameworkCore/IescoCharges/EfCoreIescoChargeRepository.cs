using Billing.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Billing.IescoCharges;

public class EfCoreIescoChargeRepository : EfCoreRepository<BillingDbContext, IescoCharge, Guid>, IIescoChargeRepository
{
    public EfCoreIescoChargeRepository(IDbContextProvider<BillingDbContext> dbContextProvider) : base(dbContextProvider) { }
    public async Task<List<IescoCharge>> GetListAsync(
       int skipCount,
       int maxResultCount,
       string sorting,
       string? filter
       )
    {
        var dbSet = await GetDbSetAsync();
        var query = dbSet.Include(x => x.LastModifier).AsQueryable();

        if (!string.IsNullOrWhiteSpace(filter))
        {
            query = query.Where(g =>
                (g.TotalEnergyCharges.ToString() ?? "").Contains(filter) ||
                g.IescoFixCharges.ToString().Contains(filter) ||
                g.ServiceRent.ToString().Contains(filter) ||
                g.VarFpa.ToString().Contains(filter) ||
                g.QtrTariffAdj.ToString().Contains(filter) ||
                g.TotalIescoCharges.ToString().Contains(filter)
            );
        }
        query = query.OrderBy(sorting ?? "Id");
        return await query
            .Skip(skipCount)
            .Take(maxResultCount)
            .ToListAsync();

    }

    public async Task<long> GetCountAsync(
        string? filter
        )
    {
        var dbSet = await GetDbSetAsync();
        var query = dbSet.AsQueryable();
        if (!string.IsNullOrWhiteSpace(filter))
        {
            query = query.Where(g =>
                g.TotalEnergyCharges.ToString().Contains(filter) ||
                g.IescoFixCharges.ToString().Contains(filter) ||
                g.ServiceRent.ToString().Contains(filter) ||
                g.VarFpa.ToString().Contains(filter) ||
                g.QtrTariffAdj.ToString().Contains(filter) ||
                g.TotalIescoCharges.ToString().Contains(filter)
            );
        }
        return await query.LongCountAsync();
    }

    public async Task<decimal?> GetTotalIescoCharges()
    {
        var dbSet = await GetDbSetAsync();

        return await dbSet
            .OrderByDescending(x => x.CreationTime)
            .Select(x => x.TotalIescoCharges)
            .FirstOrDefaultAsync();
    }
}
