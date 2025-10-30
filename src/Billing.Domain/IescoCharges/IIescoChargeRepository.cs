using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace Billing.IescoCharges;

public interface IIescoChargeRepository : IRepository<IescoCharge, Guid>
{
    Task<List<IescoCharge>> GetListAsync(
        int skipCount,
        int maxResultCount,
        string sorting,
        string? filter
        );

    Task<long> GetCountAsync(
        string? filter
        );
}
