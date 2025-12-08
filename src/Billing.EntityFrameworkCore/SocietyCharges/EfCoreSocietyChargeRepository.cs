using Billing.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Billing.SocietyCharges;

public class EfCoreSocietyChargeRepository : EfCoreRepository<BillingDbContext, SocietyCharge, Guid>, ISocietyChargeRepository
{
    public EfCoreSocietyChargeRepository(IDbContextProvider<BillingDbContext> dbContextProvider)
        : base(dbContextProvider)
    {
        public EfCoreSocietyChargeRepository(IDbContextProvider<BillingDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        public async Task<SocietyCharge?> FindByNameAsync(Guid plotSizeId)
        {
            var dbSet = await GetDbSetAsync();
            return await dbSet
                .FirstOrDefaultAsync(x => x.PlotSizeId == plotSizeId);
        }

        public async Task<List<SocietyCharge>> GetListAsync(
            int skipCount,
            int maxResultCount,
            string sorting,
            string? filter,
            Guid? plotSizeId,
            decimal? securityCharges = null,
            decimal? maintenanceCharges = null,
            decimal? waterCharges = null,
            decimal? otherCharges = null,
            decimal? totalSocietyCharges = null)
        {
            var query = await GetFilteredQueryableAsync(
                filter,
                plotSizeId,
                securityCharges,
                maintenanceCharges,
                waterCharges,
                otherCharges,
                totalSocietyCharges
            );

            if (string.IsNullOrWhiteSpace(sorting))
            {
                sorting = nameof(SocietyCharge.TotalSocietyCharges);
            }

            return await query
                .OrderBy(sorting)
                .Skip(skipCount)
                .Take(maxResultCount)
                .ToListAsync();
        }

        public async Task<long> GetCountAsync(
            string? filter,
            Guid? plotSizeId,
            decimal? securityCharges = null,
            decimal? maintenanceCharges = null,
            decimal? waterCharges = null,
            decimal? otherCharges = null,
            decimal? totalSocietyCharges = null)
        {
            var query = await GetFilteredQueryableAsync(
                filter,
                plotSizeId,
                securityCharges,
                maintenanceCharges,
                waterCharges,
                otherCharges,
                totalSocietyCharges
            );

            return await query.LongCountAsync();
        }

        private async Task<IQueryable<SocietyCharge>> GetFilteredQueryableAsync(
            string? filter,
            Guid? plotSizeId,
            decimal? securityCharges,
            decimal? maintenanceCharges,
            decimal? waterCharges,
            decimal? otherCharges,
            decimal? totalSocietyCharges)
        {
            var dbSet = await GetDbSetAsync();
            var query = dbSet
                .Include(x => x.PlotSizes)
                .AsQueryable();

            query = query
                .WhereIf(securityCharges.HasValue, x => x.SecurityCharges == securityCharges)
                .WhereIf(maintenanceCharges.HasValue, x => x.MaintenanceCharges == maintenanceCharges)
                .WhereIf(waterCharges.HasValue, x => x.WaterCharges == waterCharges)
                .WhereIf(otherCharges.HasValue, x => x.OtherCharges == otherCharges)
                .WhereIf(totalSocietyCharges.HasValue, x => x.TotalSocietyCharges == totalSocietyCharges)
                .WhereIf(plotSizeId.HasValue, x => x.PlotSizeId == plotSizeId);
                

            return query;
        }

        public async Task<SocietyCharge?> FindByPlotSizeNameAsync(string sizeName)
        {
            var dbSet = await GetDbSetAsync();

            return await dbSet
                .Include(x => x.PlotSizes)
                .FirstOrDefaultAsync(x => x.PlotSizes.SizeName == sizeName);
        }
    }

    public async Task<SocietyCharge?> FindByNameAsync(Guid plotSizeId)
    {
        var dbSet = await GetDbSetAsync();
        return await dbSet
            .FirstOrDefaultAsync(x => x.PlotSizeId == plotSizeId);
    }

    public async Task<List<SocietyCharge>> GetListAsync(
        int skipCount,
        int maxResultCount,
        string sorting,
        string? filter,
        Guid? plotSizeId,
        decimal? securityCharges = null,
        decimal? maintenanceCharges = null,
        decimal? waterCharges = null,
        decimal? otherCharges = null,
        decimal? totalSocietyCharges = null)
    {
        var query = await GetFilteredQueryableAsync(
            filter,
            plotSizeId,
            securityCharges,
            maintenanceCharges,
            waterCharges,
            otherCharges,
            totalSocietyCharges
        );

        if (string.IsNullOrWhiteSpace(sorting))
        {
            sorting = nameof(SocietyCharge.TotalSocietyCharges);
        }

        return await query
            .OrderBy(sorting)
            .Skip(skipCount)
            .Take(maxResultCount)
            .ToListAsync();
    }

    public async Task<long> GetCountAsync(
        string? filter,
        Guid? plotSizeId,
        decimal? securityCharges = null,
        decimal? maintenanceCharges = null,
        decimal? waterCharges = null,
        decimal? otherCharges = null,
        decimal? totalSocietyCharges = null)
    {
        var query = await GetFilteredQueryableAsync(
            filter,
            plotSizeId,
            securityCharges,
            maintenanceCharges,
            waterCharges,
            otherCharges,
            totalSocietyCharges
        );

        return await query.LongCountAsync();
    }

    private async Task<IQueryable<SocietyCharge>> GetFilteredQueryableAsync(
        string? filter,
        Guid? plotSizeId,
        decimal? securityCharges,
        decimal? maintenanceCharges,
        decimal? waterCharges,
        decimal? otherCharges,
        decimal? totalSocietyCharges)
    {
        var dbSet = await GetDbSetAsync();
        var query = dbSet
            .Include(x => x.PlotSizes)
            .AsQueryable();

        query = query
            .WhereIf(securityCharges.HasValue, x => x.SecurityCharges == securityCharges)
            .WhereIf(maintenanceCharges.HasValue, x => x.MaintenanceCharges == maintenanceCharges)
            .WhereIf(waterCharges.HasValue, x => x.WaterCharges == waterCharges)
            .WhereIf(otherCharges.HasValue, x => x.OtherCharges == otherCharges)
            .WhereIf(totalSocietyCharges.HasValue, x => x.TotalSocietyCharges == totalSocietyCharges)
            .WhereIf(plotSizeId.HasValue, x => x.PlotSizeId == plotSizeId);
            

        return query;
    }

    public async Task<SocietyCharge?> FindByPlotSizeNameAsync(string sizeName)
    {
        var dbSet = await GetDbSetAsync();
        return await dbSet
       .Include(x => x.PlotSizes)
       .FirstOrDefaultAsync(x => x.PlotSizes.SizeName == sizeName);
    }


}
