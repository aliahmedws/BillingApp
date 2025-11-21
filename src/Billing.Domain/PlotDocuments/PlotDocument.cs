using Billing.FileAttachments;
using Billing.PlotInfos;
using System;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace Billing.PlotDocuments;

public class PlotDocument : FullAuditedAggregateRoot<Guid>, IMultiTenant
{
    public Guid PlotInfoId { get; set; }
    public string? Description { get; set; }
    public string? DocumentNumber { get; set; }
    public DateTime? IssueDate { get; set; }
    public DateTime? ExpireDate { get; set; }
    public PlotDocumentType PlotDocumentType { get; set; }
    public bool IsVerified { get; set; } = false;

    public virtual PlotInfo PlotInfo { get; set; }
    public virtual FileAttachment? FileAttachments { get; set; }

    public Guid? TenantId { get; set; }

    private PlotDocument() { }

    internal PlotDocument(
        Guid id,
        Guid plotInfoId,
        string? description,
        string? documentNumber,
        DateTime? issueDate,
        DateTime? expireDate,
        PlotDocumentType plotDocumentType,
        bool isVerified,
        FileAttachment? fileAttachments) : base(id)
    {
        PlotInfoId = plotInfoId;
        SetDescription(description);
        SetDocumentNumber(documentNumber);
        IssueDate = issueDate;
        ExpireDate = expireDate;
        PlotDocumentType = plotDocumentType;
        IsVerified = isVerified;
        FileAttachments = fileAttachments;
    }

    public PlotDocument ChangeDescription(string? newDescription)
    {
        SetDescription(newDescription);
        return this;
    }

    public PlotDocument ChangeDocumentNumber(string? documentNumber)
    {
        SetDocumentNumber(documentNumber);
        return this;
    }

    public PlotDocument ChangeFileAttachment(FileAttachment newAttachment)
    {
        FileAttachments = newAttachment ?? throw new ArgumentNullException(nameof(newAttachment));
        return this;
    }

    public PlotDocument ChangeDocumentType(PlotDocumentType newType)
    {
        PlotDocumentType = newType;
        return this;
    }

    private void SetDescription(string? description)
    {
        if (description.IsNullOrWhiteSpace())
        {
            Description = null;
            return;
        }

        Description = Check.NotNullOrWhiteSpace(description, nameof(description), maxLength: PlotInfoConsts.MaxRemarksLength);
    }

    private void SetDocumentNumber(string? documentNumber)
    {
        if (documentNumber.IsNullOrWhiteSpace())
        {
            DocumentNumber = null;
            return;
        }

        DocumentNumber = Check.NotNullOrWhiteSpace(documentNumber, nameof(documentNumber), maxLength: PlotInfoConsts.MaxDocumentNumber);
    }

}
