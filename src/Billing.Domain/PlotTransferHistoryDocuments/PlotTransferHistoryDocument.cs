using Billing.FileAttachments;
using Billing.PlotDocuments;
using Billing.PlotTransferHistories;
using System;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace Billing.PlotTransferHistoryDocuments;

public class PlotTransferHistoryDocument : FullAuditedAggregateRoot<Guid>, IMultiTenant
{
    public Guid? TenantId { get; set; }

    public Guid PlotTransferHistoryId { get; set; }
    public PlotHistoryDocumentType PlotHistoryDT { get; set; }

    public string? RegistryNo { get; set; }

    public DateTime? IssueDate { get; set; }
    public DateTime? ExpireDate { get; set; }

    public string? Remarks { get; set; }

    public bool IsVerified { get; set; }

    public FileAttachment? FileAttachments { get; set; }
    public virtual PlotTransferHistory? PlotTransferHistories { get; set; }

    private PlotTransferHistoryDocument() { }

    internal PlotTransferHistoryDocument(
        Guid id,
        Guid plotTransferHistoryId,
        PlotHistoryDocumentType plotHistoryDT,
        string? registryNo,
        DateTime? issueDate,
        DateTime? expireDate,
        string? remarks,
        bool isVerified,
        FileAttachment? fileAttachments) : base(id)
    {
        PlotTransferHistoryId = plotTransferHistoryId;
        PlotHistoryDT = plotHistoryDT;
        RegistryNo = registryNo;
        IssueDate = issueDate;
        ExpireDate = expireDate;
        Remarks = remarks;
        IsVerified = isVerified;
        FileAttachments = fileAttachments;
    }

    public PlotTransferHistoryDocument ChangeRemarks(string? remarks)
    {
        SetRemarks(remarks);
        return this;
    }

    private void SetRemarks(string? remarks)
    {
        if (remarks.IsNullOrWhiteSpace())
        {
            Remarks = null;
            return;
        }

        Remarks = Check.Length(remarks, nameof(remarks), PlotTransferHistoryConsts.MaxRemarksLength, 0);
    }
}
