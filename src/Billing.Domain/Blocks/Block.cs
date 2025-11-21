using Billing.MeterInfos;
using Billing.Phases;
using Billing.PlotInfos;
using System;
using System.Collections.Generic;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace Billing.Blocks;

public class Block : FullAuditedAggregateRoot<Guid>, IMultiTenant
{
    public string BlockCode { get; set; }
    public string BlockName { get; set; }
    public string? Description { get; set; }
    public Guid PhaseId { get; set; }
    public Phase Phases { get; set; }
    public bool IsActive { get; set; } = true;
    public virtual ICollection<PlotInfo> PlotInfos { get; set; }
    public virtual ICollection<MeterInfo> MeterInfos { get; set; }
    public Guid? TenantId { get; set; }

    private Block()
    {
        PlotInfos = new List<PlotInfo>();
        MeterInfos = new List<MeterInfo>();
    }

    internal Block(
        Guid id,
        string blockCode,
        string blockName,
        Guid phaseId,
        string? description = null,
        bool isActive = true,
        Guid? tenantId = null)
        : base(id)
    {
        SetBlockCode(blockCode);
        SetBlockName(blockName);
        SetDescription(description);
        PhaseId = phaseId;
        IsActive = isActive;
        TenantId = tenantId;
    }

    internal Block ChangeBlockCode(string blockCode)
    {
        SetBlockCode(blockCode!);
        return this;
    }

    internal Block ChangeBlockName(string blockName)
    {
        SetBlockName(blockName);
        return this;
    }

    internal Block ChangeDescription(string? description)
    {
        SetDescription(description);
        return this;
    }

    internal Block SetPhase(Guid phaseId)
    {
        PhaseId = phaseId;
        return this;
    }

    internal Block SetTenant(Guid? tenantId)
    {
        TenantId = tenantId;
        return this;
    }

    internal Block SetActiveStatus(bool isActive)
    {
        IsActive = isActive;
        return this;
    }

    private void SetBlockCode(string blockCode)
    {
        BlockCode = Check.NotNullOrWhiteSpace(
            blockCode,
            nameof(blockCode),
            maxLength: BlockConsts.MaxBlockCodeLength
        );
    }

    private void SetBlockName(string blockName)
    {
        BlockName = Check.NotNullOrWhiteSpace(
            blockName,
            nameof(blockName),
            maxLength: BlockConsts.MaxBlockNameLength
        );
    }

    private void SetDescription(string? description)
    {
        if (!description.IsNullOrWhiteSpace())
        {
            Description = Check.Length(
                description,
                nameof(description),
                BlockConsts.MaxDescriptionLength,
                0
            );
        }
        else
        {
            Description = null;
        }
    }
}

