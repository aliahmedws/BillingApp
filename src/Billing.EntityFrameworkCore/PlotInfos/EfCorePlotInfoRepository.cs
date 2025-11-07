using Billing.Blocks;
using Billing.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Billing.PlotInfos;

public class EfCorePlotInfoRepository : EfCoreRepository<BillingDbContext, PlotInfo, Guid>, IPlotInfoRepository
{
    public EfCorePlotInfoRepository(IDbContextProvider<BillingDbContext> dbContextProvider)
        : base(dbContextProvider)
    {
    }

    public async Task<PlotInfo?> FindByPlotNoAsync(string plotNo, Guid blockId)
    {
        var dbSet = await GetDbSetAsync();
        return await dbSet.FirstOrDefaultAsync(x => x.PlotNo == plotNo && x.BlockId == blockId);
    }

    public async Task<PlotInfo?> FindByConsumerAsync(Guid consumerId, Guid blockId, Guid phaseId)
    {
        var dbSet = await GetDbSetAsync();
        return await dbSet.FirstOrDefaultAsync(
            x => x.ConsumerId == consumerId && x.BlockId == blockId && x.PhaseId == phaseId
        );
    }

    public async Task<long> GetCountAsync(
        string? filter,
        string? plotNo,
        string? streetNo,
        Guid? blockId,
        Guid? phaseId,
        Guid? plotSizeId,
        PlotStatus? status,
        Guid? consumerId)
    {
        var data = await GetFilterAsync(filter, plotNo, streetNo, blockId, phaseId, plotSizeId, status, consumerId);
        return await data.LongCountAsync();
    }

    public async Task<List<PlotInfo>> GetListAsync(
        int skipCount,
        int maxResultCount,
        string sorting,
        string? filter,
        string? plotNo,
        string? streetNo,
        Guid? blockId,
        Guid? phaseId,
        Guid? plotSizeId,
        PlotStatus? status,
        Guid? consumerId)
    {
        var data = await GetFilterAsync(filter, plotNo, streetNo, blockId, phaseId, plotSizeId, status, consumerId);

        return await data
            .OrderBy(sorting)
            .PageBy(skipCount, maxResultCount)
            .ToListAsync();
    }

    private async Task<IQueryable<PlotInfo>> GetFilterAsync(
        string? filter,
        string? plotNo,
        string? streetNo,
        Guid? blockId,
        Guid? phaseId,
        Guid? plotSizeId,
        PlotStatus? status,
        Guid? consumerId)
    {
        var queryable = await GetQueryableAsync();

        var query = queryable
            .Include(x => x.Block)
            .Include(x => x.Phase)
            .Include(x => x.PlotSize)
            .Include(x => x.ConsumerPersonaInfo)
            .WhereIf(!filter.IsNullOrWhiteSpace(),
                x => x.PlotNo.ToLower().Contains(filter!.ToLower())
                  || x.StreetNo.ToLower().Contains(filter.ToLower()))
            .WhereIf(!plotNo.IsNullOrWhiteSpace(),
                x => x.PlotNo.ToLower().Contains(plotNo!.ToLower()))
            .WhereIf(!streetNo.IsNullOrWhiteSpace(),
                x => x.StreetNo.ToLower().Contains(streetNo!.ToLower()))
            .WhereIf(blockId.HasValue,
                x => x.BlockId == blockId)
            .WhereIf(phaseId.HasValue,
                x => x.PhaseId == phaseId)
            .WhereIf(plotSizeId.HasValue,
                x => x.PlotSizeId == plotSizeId)
            .WhereIf(status.HasValue,
                x => x.Status == status)
            .WhereIf(consumerId.HasValue,
                x => x.ConsumerId == consumerId);

        return query;
    }

    public async Task<List<PlotInfo>> GetPlotLookUpAsync()
    {
        var dbSet = await GetDbSetAsync();
        return await dbSet
            .Include(x => x.Block)
            .Include(x => x.Phase)
            .Include(x => x.PlotSize)
            .Where(x => x.Status != PlotStatus.Inactive && x.Status != PlotStatus.UnderReview)
            .ToListAsync();
    }

    public async Task ChangePlotOwnerAsync(Guid fromConsumerId, Guid toConsumerId, Guid plotId)
    {
        var dbSet = await GetDbSetAsync();
        var currentOwner = await dbSet.Where(x => x.ConsumerId == fromConsumerId && x.Id == plotId).SingleOrDefaultAsync();
        if(currentOwner == null)
        {
            throw new Exception("There is no record aganist this owner");
        }

        currentOwner.ConsumerId = toConsumerId;

        await UpdateAsync(currentOwner, autoSave: true);
    }

    public async Task<PlotInfo?> GetPlotOwnerAsync(Guid plotId)
    {
        var dbSet = await GetDbSetAsync();
        var plot = await dbSet.Include(x => x.ConsumerPersonaInfo).FirstOrDefaultAsync(x => x.Id == plotId);
        return plot;
    }

    public async Task<List<PlotInfo>> GetPlotsByBlockIdAsync(Guid blockId)
    {
        var dbSet = await GetDbSetAsync();
        var result = await dbSet
            .Include(x => x.Block)
            .Include(x => x.Phase)
            .Where(x => x.BlockId == blockId
                        && x.Status != PlotStatus.Inactive
                        && x.Status != PlotStatus.UnderReview)
            .OrderBy(x => x.PlotNo)
            .ToListAsync();
        return result.Any() ? result : new List<PlotInfo>();
    }
}

