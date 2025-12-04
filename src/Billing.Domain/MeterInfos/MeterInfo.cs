using Billing.Blocks;
using Billing.ConsumerPersonalInfos;
using Billing.ElectricityBills;
using Billing.FileAttachments;
using Billing.MeterDocuments;
using Billing.Phases;
using Billing.PlotInfos;
using System;
using System.Collections.Generic;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace Billing.MeterInfos;

public class MeterInfo : FullAuditedAggregateRoot<Guid>, IMultiTenant
{
    public string MeterNo { get; private set; }
    public MeterType MeterType { get; private set; }
    public MeterCategory MeterCategory { get; private set; }
    public MeterStatus MeterStatus { get; private set; }
    public DateTime InstallationDate { get; private set; }
    public decimal InitialReading { get; private set; }
    public Guid PhaseId { get; private set; }
    public Guid PlotId { get; private set; }
    public Guid BlockId { get; private set; }
    public Guid MeterOwnerId { get; private set; }
    public string? Remarks { get; private set; }
    public virtual Phase Phase { get; private set; }
    public virtual Block Block { get; private set; }
    public virtual PlotInfo Plot { get; private set; }
    public virtual ConsumerPersonalInfo MeterOwner { get; set; }
    public virtual ICollection<MeterDocument> MeterDocuments { get; set; }
    public virtual ICollection<ElectricityBill> ElectricityBills { get; set; }
    public Guid? TenantId { get; set; }

    private MeterInfo() 
    {
        MeterDocuments = new List<MeterDocument>();
        ElectricityBills = new List<ElectricityBill>();
    }

    internal MeterInfo(
        Guid id,
        string meterNo,
        MeterType meterType,
        MeterCategory meterCategory,
        MeterStatus meterStatus,
        DateTime installationDate,
        decimal initialReading,
        Guid phaseId,
        Guid blockId,
        Guid plotId,
        Guid meterOwnerId,
        string? remarks = null,
        Guid? tenantId = null)
        : base(id)
    {
        SetMeterNo(meterNo);
        MeterType = meterType;
        MeterCategory = meterCategory;
        MeterStatus = meterStatus;
        InstallationDate = installationDate;
        InitialReading = Check.Range(initialReading, nameof(initialReading), MeterInfoConsts.MinInitialReading, decimal.MaxValue);
        PhaseId = Check.NotNull(phaseId, nameof(phaseId));
        BlockId = Check.NotNull(blockId, nameof(blockId));
        PlotId = Check.NotNull(plotId, nameof(plotId));
        MeterOwnerId = Check.NotNull(meterOwnerId, nameof(meterOwnerId));
        SetRemarks(remarks);
        TenantId = tenantId;
    }

    internal MeterInfo ChangeMeterOwner(Guid meterOwnerId)
    {
        MeterOwnerId = meterOwnerId;
        return this;
    }

    internal MeterInfo ChangePhase(Guid phaseId)
    {
        PhaseId = phaseId;
        return this;
    }

    internal MeterInfo ChangePlot(Guid plotId)
    {
        PlotId = plotId;
        return this;
    }

    internal MeterInfo ChangeBlock(Guid blockId)
    {
        BlockId = blockId;
        return this;
    }

    internal MeterInfo ChangeStatus(MeterStatus newStatus)
    {
        MeterStatus = newStatus;
        return this;
    }

    internal MeterInfo SetTenant(Guid? tenantId)
    {
        TenantId = tenantId;
        return this;
    }

    internal MeterInfo ChangeType(MeterType newType)
    {
        MeterType = newType;
        return this;
    }

    internal MeterInfo ChangeMeterNo(string meterNo)
    {
        MeterNo = meterNo;
        return this;
    }

    internal MeterInfo ChangeInstallationDate(DateTime installationDate)
    {
        InstallationDate = installationDate;
        return this;
    }

    internal MeterInfo ChangeMeterCategory(MeterCategory meterCategory)
    {
        MeterCategory = meterCategory;
        return this;
    }

    internal MeterInfo ChangeInitialReading(decimal newReading)
    {
        InitialReading = Check.Range(newReading, nameof(newReading), MeterInfoConsts.MinInitialReading, decimal.MaxValue);
        return this;
    }

    internal MeterInfo ChangeRemarks(string? remarks)
    {
        SetRemarks(remarks);
        return this;
    }

    private void SetMeterNo(string meterNo)
    {
        MeterNo = Check.NotNullOrWhiteSpace(meterNo, nameof(meterNo), maxLength: MeterInfoConsts.MaxMeterNoLength);
    }

    private void SetRemarks(string? remarks)
    {
        if (!remarks.IsNullOrWhiteSpace())
        {
            Remarks = Check.Length(remarks, nameof(remarks), MeterInfoConsts.MaxRemarksLength, 0);
        }
        else
        {
            Remarks = null;
        }
    }
}
