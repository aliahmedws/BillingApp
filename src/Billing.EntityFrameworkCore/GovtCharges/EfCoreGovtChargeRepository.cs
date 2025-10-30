using Billing.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Billing.GovtCharges;

public class EfCoreGovtChargeRepository : EfCoreRepository<BillingDbContext, GovtCharge, Guid>, IGovtChargeRepository
{
    public EfCoreGovtChargeRepository(IDbContextProvider<BillingDbContext> dbContextProvider) : base(dbContextProvider) { }

    public async Task<List<GovtCharge>> GetListAsync(
        int skipCount,
        int maxResultCount,
        string sorting,
        string? filter
        )
    {
        var dbSet = await GetDbSetAsync();
        var query = dbSet.AsQueryable();
        if (!string.IsNullOrWhiteSpace(filter))
        {
            query = query.Where(g =>
                g.Ed.ToString().Contains(filter) ||
                g.TvFee.ToString().Contains(filter) ||
                g.GST.ToString().Contains(filter) ||
                g.IncomeTax.ToString().Contains(filter) ||
                g.ExtraTax.ToString().Contains(filter) ||
                g.FurtherTax.ToString().Contains(filter) ||
                g.NjSurcharge.ToString().Contains(filter) ||
                g.SalesTax.ToString().Contains(filter) ||
                g.FcSurcharge.ToString().Contains(filter) ||
                g.TrSurcharge.ToString().Contains(filter) ||
                g.TaxOnFpa.ToString().Contains(filter) ||
                g.TotalTaxes.ToString().Contains(filter)
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
                g.Ed.ToString().Contains(filter) ||
                g.TvFee.ToString().Contains(filter) ||
                g.GST.ToString().Contains(filter) ||
                g.IncomeTax.ToString().Contains(filter) ||
                g.ExtraTax.ToString().Contains(filter) ||
                g.FurtherTax.ToString().Contains(filter) ||
                g.NjSurcharge.ToString().Contains(filter) ||
                g.SalesTax.ToString().Contains(filter) ||
                g.FcSurcharge.ToString().Contains(filter) ||
                g.TrSurcharge.ToString().Contains(filter) ||
                g.TaxOnFpa.ToString().Contains(filter) ||
                g.TotalTaxes.ToString().Contains(filter)
            );
        }
        return await query.LongCountAsync();
    }
}
