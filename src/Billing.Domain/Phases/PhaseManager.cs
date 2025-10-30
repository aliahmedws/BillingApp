using System;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Domain.Services;

namespace Billing.Phases;

public class PhaseManager : DomainService
{
    private readonly IPhaseRepository _phaseRepository;

    public PhaseManager(IPhaseRepository phaseRepository)
    {
        _phaseRepository = phaseRepository;
    }

    public async Task<Phase> CreateAsync(
        string phaseCode,
        string phaseName,
        string? description = null,
        bool isActive = true)
    {
        Check.NotNullOrWhiteSpace(phaseCode, nameof(phaseCode));
        Check.NotNullOrWhiteSpace(phaseName, nameof(phaseName));

        var existingPhase = await _phaseRepository.FindByNameAsync(phaseName);
        if (existingPhase != null)
        {
            throw new PhaseAlreadyExistsException(phaseName);
        }

        var existingCode = await _phaseRepository.FindByCodeAsync(phaseCode);
        if(existingCode != null)
        {
            throw new PhaseCodeAlreadyExistsException(phaseCode!);
        }

        return new Phase(
            GuidGenerator.Create(),
            phaseCode,
            phaseName,
            description,
            isActive
        );
    }

    public async Task UpdateAsync(
     Phase phase,
     string newCode,
     string newName,
     string? newDescription,
     bool isActive)
    {
        Check.NotNull(phase, nameof(phase));
        Check.NotNullOrWhiteSpace(newName, nameof(newName));
        Check.NotNullOrWhiteSpace(newCode, nameof(newCode));

        var existingPhase = await _phaseRepository.FindByNameAsync(newName);
        if (existingPhase != null && existingPhase.Id != phase.Id)
        {
            throw new PhaseAlreadyExistsException(newName);
        }

        if (!newCode.IsNullOrWhiteSpace())
        {
            var existingByCode = await _phaseRepository.FindByCodeAsync(newCode);
            if (existingByCode != null && existingByCode.Id != phase.Id)
            {
                throw new PhaseCodeAlreadyExistsException(newCode);
            }
        }

        phase
            .ChangePhaseCode(newCode)
            .ChangePhaseName(newName)
            .ChangeDescription(newDescription)
            .SetActiveStatus(isActive);
    }

}
