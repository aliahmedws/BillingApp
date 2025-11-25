using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace Billing.BillingCalculations;

public interface IBillingCalculationRepository : IRepository<BillingCalculation, Guid>
{
    Task<BillingCalculation?> FindByMeterNoAsync(Guid meterInfoId);
    Task<List<BillingCalculation>> GetListAsync(
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
   );

    Task<long> GetCountAsync(
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
        );
}
