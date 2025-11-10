using Billing.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Billing.BillingCalculations;

public class EfCoreBillingCalculationRepository
    : EfCoreRepository<BillingDbContext, BillingCalculation, Guid>, IBillingCalculationRepository
{
    public EfCoreBillingCalculationRepository(
        IDbContextProvider<BillingDbContext> dbContextProvider)
        : base(dbContextProvider)
    {
    }

    public async Task<BillingCalculation?> FindByMeterNoAsync(Guid meterInfoId)
    {
        var dbSet = await GetDbSetAsync();
        return await dbSet
            .FirstOrDefaultAsync(x => x.MeterInfoId == meterInfoId); 
    }

    public async Task<List<BillingCalculation>> GetListAsync(
       int skipCount,
       int maxResultCount,
       string sorting,
       string? filter,
       Guid? meterInfoId,
       decimal currentReading,
       decimal? previousReading,
       decimal consumedUnits,
       decimal? totalGovtCharges,
       decimal? totalIescoCharges,
       DateTime? issueDate,
       DateTime? dueDate,
       decimal? amountBeforeDueDate,
       decimal? amountAfterDueDate
    )
    {
        var query = await GetFilteredQueryableAsync(
            filter,
            meterInfoId,
            currentReading,
            previousReading,
            consumedUnits,
            totalGovtCharges,
            totalIescoCharges,
            issueDate,
            dueDate,
            amountBeforeDueDate,
            amountAfterDueDate
        );

        if (string.IsNullOrWhiteSpace(sorting))
        {
            sorting = nameof(BillingCalculation.CreationTime); 
        }

        return await query
            .OrderBy(sorting)
            .Skip(skipCount)
            .Take(maxResultCount)
            .ToListAsync();
    }

    public async Task<long> GetCountAsync(
        string? filter,
        Guid? meterInfoId,
        decimal currentReading,
        decimal? previousReading,
        decimal consumedUnits,
        decimal? totalGovtCharges,
        decimal? totalIescoCharges,
        DateTime? issueDate,
        DateTime? dueDate,
        decimal? amountBeforeDueDate,
        decimal? amountAfterDueDate
    )
    {
        var query = await GetFilteredQueryableAsync(
            filter,
            meterInfoId,
            currentReading,
            previousReading,
            consumedUnits,
            totalGovtCharges,
            totalIescoCharges,
            issueDate,
            dueDate,
            amountBeforeDueDate,
            amountAfterDueDate
        );

        return await query.LongCountAsync();
    }

    private async Task<IQueryable<BillingCalculation>> GetFilteredQueryableAsync(
        string? filter,
        Guid? meterInfoId,
        decimal currentReading,
        decimal? previousReading,
        decimal consumedUnits,
        decimal? totalGovtCharges,
        decimal? totalIescoCharges,
        DateTime? issueDate,
        DateTime? dueDate,
        decimal? amountBeforeDueDate,
        decimal? amountAfterDueDate
    )
    {
        var dbSet = await GetDbSetAsync();
        var query = dbSet
            .Include(x => x.MeterInfos)
            .AsQueryable();

        query = query
            //.WhereIf(!string.IsNullOrWhiteSpace(filter),
            //         x => x.MeterNo.Contains(filter!))
            .WhereIf(meterInfoId.HasValue,
                     x => x.MeterInfoId == meterInfoId) 
            .WhereIf(currentReading > 0,
                     x => x.CurrentReading == currentReading)
            .WhereIf(previousReading.HasValue,
                     x => x.PreviousReading == previousReading)
            .WhereIf(consumedUnits > 0,
                     x => x.ConsumedUnits == consumedUnits)
            .WhereIf(totalGovtCharges.HasValue,
                     x => x.TotalGovtCharges == totalGovtCharges)
            .WhereIf(totalIescoCharges.HasValue,
                     x => x.TotalIescoCharges == totalIescoCharges)
            .WhereIf(issueDate.HasValue,
                    x => x.IssueDate == issueDate)
            .WhereIf(dueDate.HasValue,
                    x => x.DueDate == dueDate)
            .WhereIf(amountBeforeDueDate.HasValue,
                     x => x.AmountBeforeDueDate == amountBeforeDueDate)
            .WhereIf(amountAfterDueDate.HasValue,
                     x => x.AmountAfterDueDate == amountAfterDueDate);

        return query;
    }
}
