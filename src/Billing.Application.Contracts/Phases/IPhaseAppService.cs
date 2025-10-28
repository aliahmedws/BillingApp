using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Billing.Phases;

public interface IPhaseAppService : IApplicationService
{
    Task<PhaseDto> GetAsync(Guid id);

    Task<PagedResultDto<PhaseDto>> GetListAsync(GetPhaseListDto input);

    Task<PhaseDto> CreateAsync(CreatePhaseDto input);

    Task UpdateAsync(Guid id, UpdatePhaseDto input);

    Task DeleteAsync(Guid id);
    Task<List<PhaseLookUp>> GetPhaseLookUpAsync();
}
