using Billing.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Billing.Phases;

public class EfCorePhaseRepository : EfCoreRepository<BillingDbContext, Phase, Guid>, IPhaseRepository
{
    public EfCorePhaseRepository(IDbContextProvider<BillingDbContext> dbContextProvider) : base(dbContextProvider) { }
    public async Task<Phase?> FindByCodeAsync(string phaseCode)
    {
        var dbSet = await GetDbSetAsync();
        return await dbSet.FirstOrDefaultAsync(x => x.PhaseCode == phaseCode);
    }

    public async Task<Phase?> FindByNameAsync(string phaseName)
    {
        var dbSet = await GetDbSetAsync();
        return await dbSet.FirstOrDefaultAsync(x => x.PhaseName == phaseName);
    }

    public async Task<long> GetCountAsync(string? filter, string? phaseCode, string? phaseName, bool? isActive, string? description)
    {
        var data = await GetFilterAsync(filter, phaseCode, phaseName, isActive, description);
        return await data.LongCountAsync();
    }

    public async Task<List<Phase>> GetListAsync(int skipCount, int maxResultCount, string sorting, string? filter, string? phaseCode, string? phaseName, bool? isActive, string? description)
    {
        var data = await GetFilterAsync(filter, phaseCode, phaseName, isActive, description);

        var list = await data.ToListAsync();

        if(sorting.Contains("PhaseCode", StringComparison.OrdinalIgnoreCase))
        {
            list = list.OrderBy(x =>
            {
                var parts = x.PhaseCode?.Split('-');
                return parts?.Length > 1 && int.TryParse(parts.Last(), out var num) ? num : int.MaxValue;
            }).ToList();
        }
        else if(sorting.Contains("PhaseName", StringComparison.OrdinalIgnoreCase))
        {
            list = list.OrderBy(x =>
            {
                var numberParts = new string(x.PhaseName?.Where(char.IsDigit).ToArray());
                return int.TryParse(numberParts, out var num) ? num : int.MaxValue;
            }).ToList();
        }
        else
        {
            list = list.AsQueryable().OrderBy(sorting).ToList();
        }

        return list.Skip(skipCount).Take(maxResultCount).ToList();
    }

    private async Task<IQueryable<Phase>> GetFilterAsync(
        string? filter,
        string? phaseCode,
        string? phaseName,
        bool? isActive,
        string? description)
    {
        var queryable = await GetQueryableAsync();
        var query = queryable
            .WhereIf(!filter.IsNullOrWhiteSpace(),
                  x => x.PhaseCode!.ToLower().Contains(filter!.ToLower())
                    || x.PhaseName.ToLower().Contains(filter.ToLower()))
            .WhereIf(!phaseCode.IsNullOrWhiteSpace(),
                  x => x.PhaseCode!.ToLower().Contains(phaseCode!.ToLower()))
            .WhereIf(!phaseName.IsNullOrWhiteSpace(),
                  x => x.PhaseName!.ToLower().Contains(phaseName!.ToLower()))
            .WhereIf(!description.IsNullOrWhiteSpace(),
                  x => x.Description!.ToLower().Contains(description!.ToLower()))
            .WhereIf(isActive.HasValue,
                  x => x.IsActive == isActive);

        return query;
    }
}
