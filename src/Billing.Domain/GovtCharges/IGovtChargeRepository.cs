using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace Billing.GovtCharges;

public interface IGovtChargeRepository : IRepository<GovtCharge, Guid>
{
    Task<List<GovtCharge>> GetListAsync(
        int skipCount,
        int maxResultCount,
        string sorting,
        string? filter
        );

    Task<long> GetCountAsync(
        string? filter
        );
}