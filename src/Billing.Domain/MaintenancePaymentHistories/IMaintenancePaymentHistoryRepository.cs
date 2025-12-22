using Billing.ElectricityPaymentHistories;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace Billing.MaintenancePaymentHistories;

public interface IMaintenancePaymentHistoryRepository : IRepository<MaintenancePaymentHistory, Guid>
{
    Task<MaintenancePaymentHistory?> FindByTransactionIdAsync(string transactionId);
    Task<List<MaintenancePaymentHistory>> GetListAsync(
       int skipCount,
       int maxResultCount,
       string sorting,
       Guid? maintenanceBillId = null,
       string? filter = null);
    Task<long> GetCountAsync(
       string? filter,
       PaymentMethod? method,
       DateTime? paymentDate,
       Guid? maintenanceBillId
   );
}
