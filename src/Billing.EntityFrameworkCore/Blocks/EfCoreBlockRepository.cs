using Billing.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Billing.Blocks;

public class EfCoreBlockRepository : EfCoreRepository<BillingDbContext, Block, Guid>, IBlockRepository
{
    public EfCoreBlockRepository(IDbContextProvider<BillingDbContext> dbContextProvider)
            : base(dbContextProvider)
    {
    }

    public async Task<Block?> FindByNameAsync(string blockName, Guid phaseId)
    {
        var dbSet = await GetDbSetAsync();
        return await dbSet.FirstOrDefaultAsync(x => x.BlockName == blockName && x.PhaseId == phaseId);
    }

    public async Task<Block?> FindByCodeAsync(string blockCode, Guid phaseId)
    {
        var dbSet = await GetDbSetAsync();
        return await dbSet.FirstOrDefaultAsync(x => x.BlockCode == blockCode && x.PhaseId == phaseId);
    }

    public async Task<long> GetCountAsync(
        string? filter,
        string? blockCode,
        string? blockName,
        bool? isActive,
        string? description,
        Guid? phaseId)
    {
        var data = await GetFilterAsync(filter, blockCode, blockName, isActive, description, phaseId);
        return await data.LongCountAsync();
    }

    public async Task<List<Block>> GetListAsync(
        int skipCount,
        int maxResultCount,
        string sorting,
        string? filter,
        string? blockCode,
        string? blockName,
        bool? isActive,
        string? description,
        Guid? phaseId)
    {
        var data = await GetFilterAsync(filter, blockCode, blockName, isActive, description, phaseId);

        //var list = await data
        //    .Include(x => x.Phases)
        //    .ToListAsync();

        //if (sorting.Contains("BlockCode", StringComparison.OrdinalIgnoreCase))
        //{
        //    list = list.OrderBy(x =>
        //    {
        //        var parts = x.BlockCode?.Split('-');
        //        return parts?.Length > 1 && int.TryParse(parts.Last(), out var num) ? num : int.MaxValue;
        //    }).ToList();
        //}
        //else if (sorting.Contains("BlockName", StringComparison.OrdinalIgnoreCase))
        //{
        //    list = list.OrderBy(x =>
        //    {
        //        var numberParts = new string(x.BlockName?.Where(char.IsDigit).ToArray());
        //        return int.TryParse(numberParts, out var num) ? num : int.MaxValue;
        //    }).ToList();
        //}
        //else
        //{
        //    list = list.AsQueryable().OrderBy(sorting).ToList();
        //}
        //return list.Skip(skipCount).Take(maxResultCount).ToList();
        return await data.OrderBy(sorting).PageBy(skipCount, maxResultCount).ToListAsync();
    }

    private async Task<IQueryable<Block>> GetFilterAsync(
        string? filter,
        string? blockCode,
        string? blockName,
        bool? isActive,
        string? description,
        Guid? phaseId)
    {
        var queryable = await GetQueryableAsync();

        var query = queryable
            .Include(x => x.Phases)
            .WhereIf(!filter.IsNullOrWhiteSpace(),
                x => x.BlockCode!.ToLower().Contains(filter!.ToLower())
                  || x.BlockName.ToLower().Contains(filter.ToLower()))
            .WhereIf(!blockCode.IsNullOrWhiteSpace(),
                x => x.BlockCode!.ToLower().Contains(blockCode!.ToLower()))
            .WhereIf(!blockName.IsNullOrWhiteSpace(),
                x => x.BlockName!.ToLower().Contains(blockName!.ToLower()))
            .WhereIf(!description.IsNullOrWhiteSpace(),
                x => x.Description!.ToLower().Contains(description!.ToLower()))
            .WhereIf(phaseId.HasValue && phaseId != Guid.Empty,
                x => x.PhaseId == phaseId)
            .WhereIf(isActive.HasValue,
                x => x.IsActive == isActive);

        return query;
    }
}

