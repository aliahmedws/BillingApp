using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace Billing.Phases;

public interface IPhaseRepository : IRepository<Phase, Guid>
{
    Task<Phase?> FindByNameAsync(string phaseName);
    Task<Phase?> FindByCodeAsync(string phaseCode);
    Task<List<Phase>> GetListAsync(
        int skipCount,
        int maxResultCount,
        string sorting,
        string? filter,
        string? phaseCode,
        string? phaseName,
        bool? isActive,
        string? description);
    Task<long> GetCountAsync(
        string? filter,
        string? phaseCode,
        string? phaseName,
        bool? isActive,
        string? description);
    Task<List<Phase>> GetPhaseLookUpAsync();
}
