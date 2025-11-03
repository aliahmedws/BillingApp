using Billing.Blocks;
using Billing.ConsumerPersonalInfos;
using Billing.MeterInfos;
using Billing.Phases;
using Billing.PlotSizes;
using Billing.PlotTypes;
using System;
using System.Collections.Generic;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;

namespace Billing.PlotInfos;

public class PlotInfo : FullAuditedAggregateRoot<Guid>
{
    public string PlotNo { get; set; }
    public PlotType PlotType { get; set; }
    public string StreetNo { get; set; }
    public Guid PlotSizeId { get; set; }
    public PlotStatus Status { get; set; }
    public Guid BlockId { get; set; }
    public Guid? ConsumerId { get; set; }
    public Guid PhaseId { get; set; }
    public string? Remarks { get; set; }

    // Navigation Properties
    public virtual Block Block { get; set; }
    public virtual Phase Phase { get; set; }
    public virtual PlotSize PlotSize { get; set; }
    public virtual ConsumerPersonalInfo ConsumerPersonaInfoId { get; set; }
    public virtual ICollection<MeterInfo> MeterInfos { get; set; }

    private PlotInfo()
    {
        MeterInfos = new List<MeterInfo>();
    }

    internal PlotInfo(
        Guid id,
        string plotNo,
        PlotType plotType,
        string streetNo,
        Guid plotSizeId,
        PlotStatus status,
        Guid blockId,
        Guid? consumerId,
        Guid phaseId,
        string? remarks = null)
        : base(id)
    {
        SetPlotNo(plotNo);
        SetStreetNo(streetNo);
        PlotType = plotType;
        PlotSizeId = Check.NotNull(plotSizeId, nameof(plotSizeId));
        Status = status;
        BlockId = Check.NotNull(blockId, nameof(blockId));
        ConsumerId = consumerId;
        PhaseId = Check.NotNull(phaseId, nameof(phaseId));
        SetRemarks(remarks);
    }

    internal PlotInfo ChangePlotNo(string plotNo)
    {
        SetPlotNo(plotNo);
        return this;
    }

    internal PlotInfo ChangeStreetNo(string streetNo)
    {
        SetStreetNo(streetNo);
        return this;
    }

    internal PlotInfo ChangePlotType(PlotType plotType)
    {
        PlotType = plotType;
        return this;
    }

    internal PlotInfo ChangePlotSize(Guid plotSizeId)
    {
        PlotSizeId = Check.NotNull(plotSizeId, nameof(plotSizeId));
        return this;
    }

    internal PlotInfo ChangeStatus(PlotStatus status)
    {
        Status = status;
        return this;
    }

    internal PlotInfo ChangeBlock(Guid blockId)
    {
        BlockId = Check.NotNull(blockId, nameof(blockId));
        return this;
    }

    internal PlotInfo ChangeBuyer(Guid? consumerid)
    {
        ConsumerId = consumerid;
        return this;
    }

    internal PlotInfo ChangePhase(Guid phaseId)
    {
        PhaseId = Check.NotNull(phaseId, nameof(phaseId));
        return this;
    }

    internal PlotInfo ChangeRemarks(string? remarks)
    {
        SetRemarks(remarks);
        return this;
    }

    // --- Private Setters ---
    private void SetPlotNo(string plotNo)
    {
        PlotNo = Check.NotNullOrWhiteSpace(plotNo, nameof(PlotNo), maxLength: PlotInfoConsts.MaxPlotNoLength);
    }

    private void SetStreetNo(string streetNo)
    {
        StreetNo = Check.NotNullOrWhiteSpace(streetNo, nameof(StreetNo), maxLength: PlotInfoConsts.MaxStreetNoLength);
    }

    private void SetRemarks(string? remarks)
    {
        if (!remarks.IsNullOrWhiteSpace())
        {
            Remarks = Check.Length(remarks, nameof(Remarks), PlotInfoConsts.MaxRemarksLength, 0);
        }
        else
        {
            Remarks = null;
        }
    }
}
