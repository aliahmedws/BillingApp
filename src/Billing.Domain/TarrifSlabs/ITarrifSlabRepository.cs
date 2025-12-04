using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace Billing.TarrifSlabs;

public interface ITarrifSlabRepository : IRepository<TarrifSlab, Guid>
{
    Task<TarrifSlab> FindByLowerSlab(decimal lowerSlab);
    Task<TarrifSlab?> FindByUpperSlab(decimal? upperSlab);
    Task<TarrifSlab> FindByUnitPrice(decimal unitPrice);
    Task<List<TarrifSlab>> GetListAsync(
        int skipCount,
        int maxResultCount,
        string sorting,
        string? filter,
        decimal? lowerSlab,
        decimal? upperSlab,
        decimal? unitPrice
    );
    Task<long> GetCountAsync(string? filter, decimal? lowerSlab, decimal? upperSlab, decimal? unitPrice);
}
