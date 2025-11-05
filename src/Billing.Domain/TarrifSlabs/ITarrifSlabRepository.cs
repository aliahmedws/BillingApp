using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace Billing.TarrifSlabs;

public interface ITarrifSlabRepository : IRepository<TarrifSlab, Guid>
{
    Task<List<TarrifSlab>> GetListAsync(
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
        );

    Task<long> GetCountAsync(
        string? filter,
        decimal rateRangeOne,
        decimal rateRangeTwo,
        decimal rateRangeThree,
        decimal rateRangeFour,
        decimal? rateRangeFive,
        decimal? rateRangeSix,
        decimal? rateRangeSeven,
        decimal? rateRangeEight
        );
}
