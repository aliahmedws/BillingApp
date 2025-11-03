using Billing.Phases;
using Billing.PlotInfos;
using System;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;

namespace Billing.MeterInfos;

public class MeterInfo : FullAuditedAggregateRoot<Guid>
{
    public string MeterNo { get; private set; }
    public MeterType MeterType { get; private set; }
    public MeterCategory MeterCategory { get; private set; }
    public MeterStatus MeterStatus { get; private set; }
    public DateTime InstallationDate { get; private set; }
    public decimal InitialReading { get; private set; }
    public Guid PhaseId { get; private set; }
    public Guid PlotId { get; private set; }
    public string? Remarks { get; private set; }

    public virtual Phase Phase { get; private set; }
    public virtual PlotInfo Plot { get; private set; }

    private MeterInfo() { }

    internal MeterInfo(
        Guid id,
        string meterNo,
        MeterType meterType,
        MeterCategory meterCategory,
        MeterStatus meterStatus,
        DateTime installationDate,
        decimal initialReading,
        Guid phaseId,
        Guid plotId,
        string? remarks = null)
        : base(id)
    {
        SetMeterNo(meterNo);
        MeterType = meterType;
        MeterCategory = meterCategory;
        MeterStatus = meterStatus;
        InstallationDate = installationDate;
        InitialReading = Check.Range(initialReading, nameof(initialReading), MeterInfoConsts.MinInitialReading, decimal.MaxValue);
        PhaseId = Check.NotNull(phaseId, nameof(phaseId));
        PlotId = Check.NotNull(plotId, nameof(plotId));
        SetRemarks(remarks);
    }

    internal MeterInfo ChangeStatus(MeterStatus newStatus)
    {
        MeterStatus = newStatus;
        return this;
    }

    internal MeterInfo ChangeType(MeterType newType)
    {
        MeterType = newType;
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
