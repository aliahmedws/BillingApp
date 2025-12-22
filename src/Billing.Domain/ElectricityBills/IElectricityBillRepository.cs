using Billing.MaintenanceBills;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace Billing.ElectricityBills;

public interface IElectricityBillRepository : IRepository<ElectricityBill, Guid>
{
    Task<ElectricityBill> FindByMeterAndBillingMonthAsync(Guid meterInfoId, DateTime billingMonth);
    Task<List<ElectricityBill>> GetListAsync(
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
        BillStatus? status
        );

    Task<long> GetCountAsync(
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
        BillStatus? status
        );

    Task<ElectricityBill?> GetByIdAsync(Guid id);
    Task<List<ElectricityBill>> GetLastTenBillsAsync(Guid meterId, Guid consumerId, int maxRecords);
}
