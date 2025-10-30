using Billing.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Billing.SocietyCharges
{
    public class EfCoreSocietyChargeRepository : EfCoreRepository<BillingDbContext, SocietyCharge, Guid>, ISocietyChargeRepository
    {
        public EfCoreSocietyChargeRepository(IDbContextProvider<BillingDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        public async Task<List<SocietyCharge>> GetListAsync(
            int skipCount,
            int maxResultCount,
            string sorting,
            string? filter,
            decimal? securityCharges = null,
            decimal? maintenanceCharges = null,
            decimal? waterCharges = null,
            decimal? otherCharges = null,
            decimal? totalSocietyCharges = null)
        {
            var query = await GetFilteredQueryableAsync(
                filter,
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
            decimal? securityCharges = null,
            decimal? maintenanceCharges = null,
            decimal? waterCharges = null,
            decimal? otherCharges = null,
            decimal? totalSocietyCharges = null)
        {
            var query = await GetFilteredQueryableAsync(
                filter,
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
            decimal? securityCharges,
            decimal? maintenanceCharges,
            decimal? waterCharges,
            decimal? otherCharges,
            decimal? totalSocietyCharges)
        {
            var dbSet = await GetDbSetAsync();
            var query = dbSet.AsQueryable();

            query = query
                .WhereIf(securityCharges.HasValue, x => x.SecurityCharges == securityCharges)
                .WhereIf(maintenanceCharges.HasValue, x => x.MaintenanceCharges == maintenanceCharges)
                .WhereIf(waterCharges.HasValue, x => x.WaterCharges == waterCharges)
                .WhereIf(otherCharges.HasValue, x => x.OtherCharges == otherCharges)
                .WhereIf(totalSocietyCharges.HasValue, x => x.TotalSocietyCharges == totalSocietyCharges);

            return query;
        }
    }
}
