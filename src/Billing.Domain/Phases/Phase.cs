using Billing.Blocks;
using Billing.MeterInfos;
using Billing.PlotInfos;
using System;
using System.Collections.Generic;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace Billing.Phases;

public class Phase : FullAuditedAggregateRoot<Guid>, IMultiTenant
{
    public string PhaseCode { get; set; }
    public string PhaseName { get; set; }
    public string? Description { get; set; }
    public bool IsActive { get; set; } = true;
    public virtual ICollection<Block> Blocks { get; set; }
    public virtual ICollection<PlotInfo> PlotInfos { get; set; }
    public virtual ICollection<MeterInfo> MeterInfos { get; set; }

    public Guid? TenantId { get; set; }

    private Phase()
    {
        Blocks = new List<Block>();
        PlotInfos = new List<PlotInfo>();
        MeterInfos = new List<MeterInfo>();
    }

    internal Phase(
        Guid id,
        string phaseCode,
        string phaseName,
        string? description = null,
        bool isActive = true,
        Guid? tenantId = null)
        : base(id)
    {
        SetPhaseCode(phaseCode!);
        SetPhaseName(phaseName);
        ChangeDescription(description!);
        IsActive = isActive;
        TenantId = tenantId;
    }

    internal Phase ChangePhaseCode(string phaseCode)
    {
        SetPhaseCode(phaseCode);
        return this;
    }

    internal Phase SetTenant(Guid? tenantId)
    {
        TenantId = tenantId;
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

    private void SetPhaseCode(string phaseCode)
    {
        PhaseCode = Check.NotNullOrWhiteSpace(
            phaseCode,
            nameof(phaseCode),
            maxLength: PhaseConsts.MaxPhaseCodeLength);
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

