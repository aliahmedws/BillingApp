using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace Billing.MaintenanceBills;

public interface IMaintenanceBillRepository : IRepository<MaintenanceBill, Guid>
{
    Task<MaintenanceBill?> FindByPlotAndBillingMonthAsync(Guid plotInfoId, DateTime billingMonth);

    Task<long> GetCountAsync(
        string? filter,
        BillStatus? status,
        DateTime? billingMonth,
        Guid? consumerId,
        Guid? plotId
    );

    Task<List<MaintenanceBill>> GetListAsync(
        int skipCount,
        int maxResultCount,
        string sorting,
        string? filter,
        BillStatus? status,
        DateTime? billingMonth,
        Guid? consumerId,
        Guid? plotId
    );
}
