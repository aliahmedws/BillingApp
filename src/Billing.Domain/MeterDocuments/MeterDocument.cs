using Billing.FileAttachments;
using Billing.MeterInfos;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using Volo.Abp.Domain.Entities.Auditing;

namespace Billing.MeterDocuments;

public class MeterDocument : FullAuditedAggregateRoot<Guid>
{
    public Guid MeterInfoId { get; set; }
    public string? Description { get; set; }
    public MeterDocumentType MeterDocumentType { get; set; }
    public FileAttachment? FileAttachments { get; set; }
    public virtual MeterInfo? MeterInfos { get; set; }

    private MeterDocument() { }

    public MeterDocument(
        Guid id,
        Guid meterInfoId,
        FileAttachment fileAttachment,
        MeterDocumentType documentType,
        string? description = null)
        : base(id)
    {
        MeterInfoId = meterInfoId;
        FileAttachments = fileAttachment ?? throw new ArgumentNullException(nameof(fileAttachment));
        MeterDocumentType = documentType;
        Description = description;
    }

    public MeterDocument ChangeDescription(string? newDescription)
    {
        Description = newDescription;
        return this;
    }

    public MeterDocument ChangeFileAttachment(FileAttachment newAttachment)
    {
        FileAttachments = newAttachment ?? throw new ArgumentNullException(nameof(newAttachment));
        return this;
    }
}

