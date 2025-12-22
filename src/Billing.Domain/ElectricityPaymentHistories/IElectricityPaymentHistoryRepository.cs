using Billing.MaintenancePaymentHistories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace Billing.ElectricityPaymentHistories;

public interface IElectricityPaymentHistoryRepository : IRepository<ElectricityPaymentHistory, Guid>
{
    Task<ElectricityPaymentHistory> FindByTransactionIdAsync(string transactionId);

    Task<ElectricityPaymentHistory?> GetLatestPaymentHistoryByBillIdAsync(Guid electricityBillId);

    Task<List<ElectricityPaymentHistory>> GetListAsync(
    int skipCount,
    int maxResultCount,
    string sorting,
    string? filter = null,
    string? transactionId = null,
    PaymentMethod? method = null,
    Guid? electricityBillId = null
    );

    Task<long> GetCountAsync(
     string? filter = null,
     string? transactionId = null,
     PaymentMethod? method = null,
     Guid? electricityBillId = null
    );

    Task<List<ElectricityPaymentHistory>> GetListByBillIdAsync(Guid electricityBillId);
}
