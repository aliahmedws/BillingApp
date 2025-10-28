using Billing.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Billing.PlotSizes;

public class EfCorePlotSizeRepository : EfCoreRepository<BillingDbContext, PlotSize, Guid>, IPlotSizeRepository
{
    public EfCorePlotSizeRepository(IDbContextProvider<BillingDbContext> dbContextProvider) : base(dbContextProvider)
    {
    }

    public async Task<long> GetCountAsync(string? filter, string? sizeName, bool? isActive, PlotUnit? plotUnit)
    {
        var data = await GetFilterAsync(filter, sizeName, isActive, plotUnit);
        return await data.LongCountAsync();
    }

    public async Task<List<PlotSize>> GetListAsync(
        int skipCount,
        int maxResultCount,
        string sorting,
        string? filter,
        string? sizeName,
        bool? isActive,
        PlotUnit? plotUnit)
    {
        var data = await GetFilterAsync(filter, sizeName, isActive, plotUnit);
        //var list = await data.ToListAsync();

        //// Handle numeric sorting inside SizeName (e.g. "5 Marla", "10 Marla")
        //if (sorting.Contains("SizeName", StringComparison.OrdinalIgnoreCase))
        //{
        //    list = list.OrderBy(x =>
        //    {
        //        var numberPart = new string(x.SizeName?.Where(char.IsDigit).ToArray());
        //        return int.TryParse(numberPart, out var num) ? num : int.MaxValue;
        //    }).ToList();
        //}
        //else
        //{
        //    list = list.AsQueryable().OrderBy(sorting).ToList();
        //}

        //return list.Skip(skipCount).Take(maxResultCount).ToList();

        return await data.OrderBy(sorting).PageBy(skipCount, maxResultCount).ToListAsync();
    }

    private async Task<IQueryable<PlotSize>> GetFilterAsync(
        string? filter,
        string? sizeName,
        bool? isActive,
        PlotUnit? plotUnit)
    {
        var queryable = await GetQueryableAsync();
        var query = queryable
            .WhereIf(!filter.IsNullOrWhiteSpace(),
                x => x.SizeName.ToLower().Contains(filter!.ToLower())
                     || x.Description!.ToLower().Contains(filter.ToLower()))
            .WhereIf(!sizeName.IsNullOrWhiteSpace(),
                x => x.SizeName.ToLower().Contains(sizeName!.ToLower()))
            .WhereIf(plotUnit.HasValue,
                x => x.Unit == plotUnit)
            .WhereIf(isActive.HasValue,
                x => x.IsActive == isActive);

        return query;
    }
}
