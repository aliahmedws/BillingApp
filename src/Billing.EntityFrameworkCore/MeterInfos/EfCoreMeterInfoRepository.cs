using Billing.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Billing.MeterInfos;


public class EfCoreMeterInfoRepository : EfCoreRepository<BillingDbContext, MeterInfo, Guid>, IMeterInfoRepository
{
    public EfCoreMeterInfoRepository(IDbContextProvider<BillingDbContext> dbContextProvider)
        : base(dbContextProvider)
    {
    }

    public async Task<MeterInfo?> FindByMeterNoAsync(string meterNo)
    {
        var dbSet = await GetDbSetAsync();
        return await dbSet.FirstOrDefaultAsync(x => x.MeterNo == meterNo);
    }

    public async Task<List<MeterInfo>> GetListAsync(
        int skipCount,
        int maxResultCount,
        string sorting,
        string? filter,
        string? meterNo,
        MeterType? meterType,
        MeterCategory? meterCategory,
        MeterStatus? meterStatus,
        DateTime? installationDate,
        Guid? phaseId,
        Guid? plotId)
    {
        var query = await GetFilteredQueryAsync(filter, meterNo, meterType, meterCategory, meterStatus, installationDate, phaseId, plotId);

        return await query
            .OrderBy(sorting)
            .PageBy(skipCount, maxResultCount)
            .ToListAsync();
    }

    public async Task<long> GetCountAsync(
        string? filter,
        string? meterNo,
        MeterType? meterType,
        MeterCategory? meterCategory,
        MeterStatus? meterStatus,
        DateTime? installationDate,
        Guid? phaseId,
        Guid? plotId)
    {
        var query = await GetFilteredQueryAsync(filter, meterNo, meterType, meterCategory, meterStatus, installationDate, phaseId, plotId);
        return await query.LongCountAsync();
    }

    private async Task<IQueryable<MeterInfo>> GetFilteredQueryAsync(
        string? filter,
        string? meterNo,
        MeterType? meterType,
        MeterCategory? meterCategory,
        MeterStatus? meterStatus,
        DateTime? installationDate,
        Guid? phaseId,
        Guid? plotId)
    {
        var queryable = await GetQueryableAsync();

        var query = queryable
            .Include(x => x.Phase)
            .Include(x => x.Plot)
            .WhereIf(!filter.IsNullOrWhiteSpace(),
                x => x.MeterNo.ToLower().Contains(filter!.ToLower())
                  || x.Remarks!.ToLower().Contains(filter.ToLower()))
            .WhereIf(!meterNo.IsNullOrWhiteSpace(),
                x => x.MeterNo.ToLower().Contains(meterNo!.ToLower()))
            .WhereIf(meterType.HasValue, x => x.MeterType == meterType)
            .WhereIf(meterCategory.HasValue, x => x.MeterCategory == meterCategory)
            .WhereIf(meterStatus.HasValue, x => x.MeterStatus == meterStatus)
            .WhereIf(installationDate.HasValue, x => x.InstallationDate.Date == installationDate!.Value.Date)
            .WhereIf(phaseId.HasValue, x => x.PhaseId == phaseId)
            .WhereIf(plotId.HasValue, x => x.PlotId == plotId);

        return query;
    }
}

