using Billing.Permissions;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Identity;

namespace Billing.Phases;

[RemoteService(isEnabled: false)]

[Authorize(BillingPermissions.Phases.Default)]
public class PhaseAppService : BillingAppService, IPhaseAppService
{
    private readonly IPhaseRepository _phaseRepository;
    private readonly PhaseManager _phaseManager;
    private readonly IIdentityUserRepository _identityUserRepository;

    public PhaseAppService(IPhaseRepository phaseRepository, PhaseManager phaseManager)
    {
        _phaseRepository = phaseRepository;
        _phaseManager = phaseManager;
    }

    [Authorize(BillingPermissions.Phases.Create)]
    public async Task<PhaseDto> CreateAsync(CreatePhaseDto input)
    {
        var phase = await _phaseManager.CreateAsync(
            input.PhaseCode,
            input.PhaseName,
            input.Description,
            input.IsActive);

        await _phaseRepository.InsertAsync(phase);
        return ObjectMapper.Map<Phase, PhaseDto>(phase);
    }

    [Authorize(BillingPermissions.Phases.Delete)]
    public async Task DeleteAsync(Guid id)
    {
        await _phaseRepository.DeleteAsync(id);
    }

    public async Task<PhaseDto> GetAsync(Guid id)
    {
        var phase = await _phaseRepository.GetAsync(id);
        return ObjectMapper.Map<Phase, PhaseDto>(phase);
    }

    public async Task<PagedResultDto<PhaseDto>> GetListAsync(GetPhaseListDto input)
    {
        if (input.Sorting.IsNullOrWhiteSpace())
        {
            input.Sorting = nameof(Phase.PhaseName);
        }

        var phases = await _phaseRepository.GetListAsync(
            input.SkipCount,
            input.MaxResultCount,
            input.Sorting,
            input.Filter,
            input.PhaseCode,
            input.PhaseName,
            input.IsActive,
            input.Description);

        var totalCount = await _phaseRepository.GetCountAsync(
            input.Filter,
            input.PhaseCode,
            input.PhaseName,
            input.IsActive,
            input.Description);

        return new PagedResultDto<PhaseDto>(
            totalCount,
            ObjectMapper.Map<List<Phase>, List<PhaseDto>>(phases));
    }

    public async Task<List<PhaseLookUp>> GetPhaseLookUpAsync()
    {
        var data = await _phaseRepository.GetPhaseLookUpAsync();
        var query = data.Select(x => new PhaseLookUp
        {
            Id = x.Id,
            PhaseName = x.PhaseName
        }).ToList();

        return query;
    }

    [Authorize(BillingPermissions.Phases.Edit)]
    public async Task UpdateAsync(Guid id, UpdatePhaseDto input)
    {
        var phase = await _phaseRepository.GetAsync(id);

        await _phaseManager.UpdateAsync(
            phase,
            input.PhaseCode,
            input.PhaseName,
            input.Description,
            input.IsActive);

        await _phaseRepository.UpdateAsync(phase);
    }
}
