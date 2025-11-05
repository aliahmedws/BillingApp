using Billing.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Billing.TarrifSlabs
{
    public class EfCoreTarrifSlabRepository
        : EfCoreRepository<BillingDbContext, TarrifSlab, Guid>, ITarrifSlabRepository
    {
        public EfCoreTarrifSlabRepository(IDbContextProvider<BillingDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        public async Task<List<TarrifSlab>> GetListAsync(
            int skipCount,
            int maxResultCount,
            string sorting,
            string? filter,
            decimal rateRangeOne,
            decimal rateRangeTwo,
            decimal rateRangeThree,
            decimal rateRangeFour,
            decimal? rateRangeFive,
            decimal? rateRangeSix,
            decimal? rateRangeSeven,
            decimal? rateRangeEight
        )
        {
            var dbSet = await GetDbSetAsync();
            var query = dbSet.AsQueryable();


          

            query = query.OrderBy(sorting ?? "Id");

            return await query
                .Skip(skipCount)
                .Take(maxResultCount)
                .ToListAsync();
        }

        public async Task<long> GetCountAsync(
            string? filter,
            decimal rateRangeOne,
            decimal rateRangeTwo,
            decimal rateRangeThree,
            decimal rateRangeFour,
            decimal? rateRangeFive,
            decimal? rateRangeSix,
            decimal? rateRangeSeven,
            decimal? rateRangeEight
        )
        {
            var dbSet = await GetDbSetAsync();
            var query = dbSet.AsQueryable();


            query = query.Where(x =>
                x.RateRangeOne == rateRangeOne &&
                x.RateRangeTwo == rateRangeTwo &&
                x.RateRangeThree == rateRangeThree &&
                x.RateRangeFour == rateRangeFour &&
                (rateRangeFive == null || x.RateRangeFive == rateRangeFive) &&
                (rateRangeSix == null || x.RateRangeSix == rateRangeSix) &&
                (rateRangeSeven == null || x.RateRangeSeven == rateRangeSeven) &&
                (rateRangeEight == null || x.RateRangeEight == rateRangeEight)
            );

            return await query.LongCountAsync();
        }
    }
}
