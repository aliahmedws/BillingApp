using Billing.EntityFrameworkCore;
using Billing.MaintenanceBills;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Billing.ElectricityBills;

public class EfCoreElectricityBillRepository : EfCoreRepository<BillingDbContext, ElectricityBill, Guid>, IElectricityBillRepository
{
    public EfCoreElectricityBillRepository(
           IDbContextProvider<BillingDbContext> dbContextProvider)
           : base(dbContextProvider)
    {
    }

    public async Task<ElectricityBill?> FindByMeterAndBillingMonthAsync(
        Guid meterInfoId,
        DateTime billingMonth)
    {
        var dbSet = await GetDbSetAsync();

        return await dbSet
            .Include(x => x.MeterInfos)
            .FirstOrDefaultAsync(x =>
                x.MeterInfoId == meterInfoId &&
                x.BillingMonth.Year == billingMonth.Year &&
                x.BillingMonth.Month == billingMonth.Month
            );
    }

    public async Task<List<ElectricityBill>> GetListAsync(
        int skipCount,
        int maxResultCount,
        string sorting,
        string? filter,
        Guid? meterInfoId,
        decimal? previousReading,
        decimal? presentReading,
        DateTime? meterReadingDate,
        DateTime? billingMonth,
        DateTime? issueDate,
        DateTime? dueDate,
        decimal? currentMonthBill,
        decimal? billAdjustment,
        decimal? anyOtherCharges,
        decimal? lpSurcharge,
        BillStatus? status)
    {
        var query = await GetFilterAsync(
            filter,
            meterInfoId,
            previousReading,
            presentReading,
            meterReadingDate,
            billingMonth,
            issueDate,
            dueDate,
            currentMonthBill,
            billAdjustment,
            anyOtherCharges,
            lpSurcharge,
            status);

        return await query
            .OrderBy(string.IsNullOrWhiteSpace(sorting)
                ? nameof(ElectricityBill.BillingMonth) + " DESC"
                : sorting)
            .PageBy(skipCount, maxResultCount)
            .ToListAsync();
    }

    public async Task<long> GetCountAsync(
        string? filter,
        Guid? meterInfoId,
        decimal? previousReading,
        decimal? presentReading,
        DateTime? meterReadingDate,
        DateTime? billingMonth,
        DateTime? issueDate,
        DateTime? dueDate,
        decimal? currentMonthBill,
        decimal? billAdjustment,
        decimal? anyOtherCharges,
        decimal? lpSurcharge,
        BillStatus? status)
    {
        var query = await GetFilterAsync(
            filter,
            meterInfoId,
            previousReading,
            presentReading,
            meterReadingDate,
            billingMonth,
            issueDate,
            dueDate,
            currentMonthBill,
            billAdjustment,
            anyOtherCharges,
            lpSurcharge,
            status);

        return await query.LongCountAsync();
    }

    private async Task<IQueryable<ElectricityBill>> GetFilterAsync(
        string? filter,
        Guid? meterInfoId,
        decimal? previousReading,
        decimal? presentReading,
        DateTime? meterReadingDate,
        DateTime? billingMonth,
        DateTime? issueDate,
        DateTime? dueDate,
        decimal? currentMonthBill,
        decimal? billAdjustment,
        decimal? anyOtherCharges,
        decimal? lpSurcharge,
        BillStatus? status)
    {
        var query = await GetQueryableAsync();

        var data = query
            .Include(x => x.MeterInfos).ThenInclude(x => x.MeterOwner)
            .WhereIf(!filter.IsNullOrWhiteSpace(),
                x =>
                    x.MeterInfos.MeterNo.ToLower().Contains(filter!.ToLower()) ||
                    x.MeterInfos.MeterOwner.FirstName.ToLower().Contains(filter!.ToLower())
            )
            .WhereIf(meterInfoId.HasValue, x => x.MeterInfoId == meterInfoId)
            .WhereIf(previousReading > 0, x => x.PreviousReading == previousReading)
            .WhereIf(presentReading > 0, x => x.PresentReading == presentReading)
            .WhereIf(meterReadingDate != default, x => x.MeterReadingDate == meterReadingDate)
            .WhereIf(billingMonth != default,
                x => x.BillingMonth.Year == billingMonth!.Value.Year &&
                     x.BillingMonth.Month == billingMonth!.Value.Month)
            .WhereIf(issueDate != default, x => x.IssueDate == issueDate)
            .WhereIf(dueDate != default, x => x.DueDate == dueDate)
            .WhereIf(currentMonthBill > 0, x => x.CurrentMonthBill == currentMonthBill)
            .WhereIf(billAdjustment > 0, x => x.BillAdjustment == billAdjustment)
            .WhereIf(anyOtherCharges > 0, x => x.AnyOtherCharges == anyOtherCharges)
            .WhereIf(lpSurcharge > 0, x => x.LPSurcharge == lpSurcharge)
            .WhereIf(status.HasValue, x => x.Status == status);

        return data;
    }

    public async Task<ElectricityBill?> GetByIdAsync(Guid id)
    {
        var dbContext = await GetDbContextAsync();

        return await dbContext.ElectricityBills
            .AsNoTracking()
            .AsSplitQuery()
            .Include(b => b.MeterInfos)
                .ThenInclude(mi => mi.MeterOwner)
            .Include(b => b.MeterInfos)
                .ThenInclude(mi => mi.Plot)
            .Include(b => b.MeterInfos)
                .ThenInclude(mi => mi.Phase)
            .Include(b => b.MeterInfos)
                .ThenInclude(mi => mi.Block)
            .FirstOrDefaultAsync(b => b.Id == id);
    }


    public async Task<List<ElectricityBill>> GetLastTenBillsAsync(Guid meterId, Guid consumerId, int maxRecords)
    {
        var dbContext = await GetDbContextAsync();

        return await dbContext.ElectricityBills
            .Include(x => x.MeterInfos)
                .Where(x => x.MeterInfoId == meterId && x.MeterInfos.MeterOwnerId == consumerId)
                    .OrderByDescending(x => x.CreationTime)
                        .Take(maxRecords)
                            .ToListAsync();
    }
}
