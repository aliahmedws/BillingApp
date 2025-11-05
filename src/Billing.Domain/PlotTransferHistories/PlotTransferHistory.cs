using Billing.ConsumerPersonalInfos;
using Billing.PlotInfos;
using System;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.Identity;
using Volo.Abp.MultiTenancy;

namespace Billing.PlotTransferHistories;

public class PlotTransferHistory : FullAuditedAggregateRoot<Guid>, IMultiTenant
{
    public Guid PlotId { get; private set; }
    public Guid FromConsumerId { get; private set; }
    public Guid ToConsumerId { get; private set; }
    public DateTime TransferDate { get; private set; }
    public TransferType TransferType { get; private set; }
    public string RegistryNo { get; private set; }
    public decimal ConsiderationAmount { get; private set; }
    public string? Remarks { get; private set; }
    public string? RejectReason { get; private set; }
    public Guid? ApprovedByUserId { get; private set; }
    public Guid? RejectByUserId { get; private set; }
    public IdentityUser? ApprovedByUser { get; private set; }
    public IdentityUser? RejectByUser { get; private set; }
    public DateTime? ApprovedAt { get; private set; }
    public bool IsApproved { get; private set; } = false;

    public virtual PlotInfo Plot { get; private set; }
    public virtual ConsumerPersonalInfo Consumers { get; private set; }
    public virtual ConsumerPersonalInfo FromConsumers { get; private set; }

    public Guid? TenantId { get; set; }

    private PlotTransferHistory() { }

    internal PlotTransferHistory(
        Guid id,
        Guid plotId,
        Guid fromConsumerId,
        Guid toConsumerId,
        DateTime transferDate,
        TransferType transferType,
        string registryNo,
        decimal considerationAmount,
        Guid? approvedByUserId = null,
        DateTime? approvedAt = null,
        bool isApproved = false,
        string? remarks = null,
        Guid? tenantId = null)
        : base(id)
    {
        PlotId = Check.NotNull(plotId, nameof(plotId));
        FromConsumerId = Check.NotNull(fromConsumerId, nameof(fromConsumerId));
        ToConsumerId = Check.NotNull(toConsumerId, nameof(toConsumerId));
        TransferDate = Check.NotNull(transferDate, nameof(transferDate));
        TransferType = transferType;
        SetRegistryNo(registryNo);
        SetConsiderationAmount(considerationAmount);
        SetRemarks(remarks);
        ApprovedByUserId = approvedByUserId;
        ApprovedAt = approvedAt;
        IsApproved = isApproved;
        TenantId = tenantId;
    }

    private void SetConsiderationAmount(decimal considerationAmount)
    {
        ConsiderationAmount = Check.Range(considerationAmount, nameof(considerationAmount), 0, decimal.MaxValue);
    }

    private void SetRegistryNo(string registryNo)
    {
        RegistryNo = Check.NotNullOrWhiteSpace(
            registryNo, nameof(registryNo),
            maxLength: PlotTransferHistoryConsts.MaxRegistryNoLength);
    }

    internal PlotTransferHistory SetTenant(Guid? tenantId)
    {
        TenantId = tenantId;
        return this;
    }


    internal PlotTransferHistory ChangeConsiderationAmount(decimal amount)
    {
        ConsiderationAmount = Check.Range(amount, nameof(amount), 0, decimal.MaxValue);
        return this;
    }

    internal PlotTransferHistory SetFromConsumer(Guid fromConsumerId)
    {
        FromConsumerId = fromConsumerId;
        return this;
    }

    internal PlotTransferHistory SetToConsumer(Guid toConsumerId)
    {
        ToConsumerId = toConsumerId;
        return this;
    }

    internal void Approve(Guid? approvedByUserId)
    {
        IsApproved = true;
        ApprovedByUserId = approvedByUserId;
        ApprovedAt = DateTime.UtcNow;
    }

    internal void Reject(string? RejectReasion = null, Guid? rejectByUserId = null)
    {
        IsApproved = false;
        ApprovedByUserId = null;
        RejectByUserId = rejectByUserId;
        ApprovedAt = null;
        SetRemarks(RejectReasion ?? "Transfer request rejected.");
    }

    private void SetRemarks(string? remarks)
    {
        if (!remarks.IsNullOrWhiteSpace())
        {
            Remarks = Check.Length(
                remarks,
                nameof(remarks),
                PlotTransferHistoryConsts.MaxRemarksLength,
                0);
        }
        else
        {
            Remarks = null;
        }
    }
}