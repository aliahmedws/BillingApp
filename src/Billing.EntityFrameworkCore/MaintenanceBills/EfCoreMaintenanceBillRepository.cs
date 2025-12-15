using Billing.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Billing.MaintenanceBills;

public class EfCoreMaintenanceBillRepository : EfCoreRepository<BillingDbContext, MaintenanceBill, Guid>, IMaintenanceBillRepository
{
    public EfCoreMaintenanceBillRepository(IDbContextProvider<BillingDbContext> dbContextProvider) : base(dbContextProvider)
    {
    }

    public async Task<MaintenanceBill?> FindByPlotAndBillingMonthAsync(
        Guid plotInfoId,
        DateTime billingMonth)
    {
        var dbSet = await GetDbSetAsync();

        return await dbSet.FirstOrDefaultAsync(x => x.PlotInfoId == plotInfoId && x.BillingMonth == billingMonth);
    }

    public async Task<long> GetCountAsync(
        string? filter,
        BillStatus? status,
        DateTime? billingMonth,
        Guid? consumerId,
        Guid? plotId)
    {
        var query = await GetFilterAsync(filter, status, billingMonth, consumerId, plotId);
        return await query.LongCountAsync();
    }

    public async Task<List<MaintenanceBill>> GetListAsync(
        int skipCount,
        int maxResultCount,
        string sorting,
        string? filter,
        BillStatus? status,
        DateTime? billingMonth,
        Guid? consumerId,
        Guid? plotId)
    {
        var query = await GetFilterAsync(filter, status, billingMonth, consumerId, plotId);

        return await query.OrderBy(sorting).PageBy(skipCount, maxResultCount).ToListAsync();
    }

    private async Task<IQueryable<MaintenanceBill>> GetFilterAsync(
        string? filter,
        BillStatus? status,
        DateTime? billingMonth,
        Guid? consumerId,
        Guid? plotId)
    {
        var queryable = await GetQueryableAsync();

        var query = queryable
            .Include(x => x.PlotInfos)
                .ThenInclude(x => x.Block).ThenInclude(x => x.Phases)
            .Include(x => x.ConsumerPersonalInfos)
            .WhereIf(!filter.IsNullOrWhiteSpace(),
                x =>(x.ConsumerPersonalInfos.FirstName + " " + x.ConsumerPersonalInfos.LastName).ToLower().Contains(filter!.ToLower())
                         || x.PlotInfos.PlotNo.ToLower().Contains(filter!.ToLower()))
            .WhereIf(status.HasValue,x => x.Status == status)
            .WhereIf(billingMonth.HasValue,x => x.BillingMonth == billingMonth!.Value)
            .WhereIf(consumerId.HasValue, x => x.ConsumerPersonalInfos.Id == consumerId)
            .WhereIf(plotId.HasValue, x => x.PlotInfos.Id == plotId);

        return query;
    }

    public async Task<List<MaintenanceBill>> GetListByIdsAsync(List<Guid> ids)
    {
        var dbContext = await GetDbContextAsync();

        return await dbContext.MaintenanceBills
            .Include(x => x.ConsumerPersonalInfos)
            .Include(x => x.PlotInfos)
            .Where(x => ids.Contains(x.Id))
            .ToListAsync();
    }

    public async Task<MaintenanceBill?> GetByIdAsync(Guid id)
    {
        var dbContext = await GetDbContextAsync();

        return await dbContext.MaintenanceBills
            .Include(x => x.ConsumerPersonalInfos)
            .Include(x => x.MaintenancePaymentHistories)
            .Include(x => x.PlotInfos).ThenInclude(x => x.Block).ThenInclude(x => x.Phases)
            .Where(x => x.Id == id)
            .FirstOrDefaultAsync();
    }

    public async Task<List<MaintenanceBill>> GetLastTenBillsAsync(
        Guid consumerId,
        Guid plotId,
        int maxRecords)
    {
        var dbContext = await GetDbContextAsync();

        return await dbContext.MaintenanceBills
            .Include(x => x.ConsumerPersonalInfos)
            .Include(x => x.PlotInfos).ThenInclude(x => x.Block).ThenInclude(x => x.Phases)
            .Where(x => x.ConsumerPersonalInfos.Id == consumerId && x.PlotInfos.Id == plotId)
            .OrderByDescending(x => x.CreationTime)
            .Take(maxRecords)
            .ToListAsync();
    }
}
