using System;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;

namespace Billing.Phases;

public class Phase : FullAuditedAggregateRoot<Guid>
{
    public string? PhaseCode { get; set; }
    public string PhaseName { get; set; }
    public string? Description { get; set; }
    public bool IsActive { get; set; } = true;

    private Phase()
    {
    }

    internal Phase(
        Guid id,
        string? phaseCode,
        string phaseName,
        string? description = null,
        bool isActive = true)
        : base(id)
    {
        SetPhaseCode(phaseCode!);
        SetPhaseName(phaseName);
        ChangeDescription(description!);
        IsActive = isActive;
    }

    internal Phase ChangePhaseCode(string? phaseCode)
    {
        SetPhaseCode(phaseCode!);
        return this;
    }

    internal Phase ChangePhaseName(string phaseName)
    {
        SetPhaseName(phaseName);
        return this;
    }

    internal Phase ChangeDescription(string? description)
    {
        SetDescription(description);
        return this;
    }

    internal Phase SetActiveStatus(bool isActive)
    {
        IsActive = isActive;
        return this;
    }

    private void SetPhaseCode(string? phaseCode)
    {
        if (!phaseCode.IsNullOrWhiteSpace())
        {
            PhaseCode = Check.Length(phaseCode, nameof(phaseCode), PhaseConsts.MaxPhaseCodeLength, 0);
        }
        else
        {
            PhaseCode = null;
        }
    }

    private void SetDescription(string? description)
    {
        if (!description.IsNullOrWhiteSpace())
        {
            Description = Check.Length(description, nameof(description), PhaseConsts.MaxDescriptionLength, 0);
        }
        else
        {
            Description = null;
        }
    }

    private void SetPhaseName(string phaseName)
    {
        PhaseName = Check.NotNullOrWhiteSpace(
            phaseName,
            nameof(phaseName),
            maxLength: PhaseConsts.MaxPhaseNameLength
        );
    }
}

