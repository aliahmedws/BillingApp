using Billing.ConsumerDocumentDetails;
using Billing.ConsumerPersonalInfos;
using Billing.FileAttachments;
using System;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace Billing.ConsumerDocuments;

public class ConsumerDocument : FullAuditedAggregateRoot<Guid>, IMultiTenant
{
    public Guid ConsumerId { get; set; }
    public ConsumerDocumentType ConsumerDT { get; set; }
    public DateTime? IssueDate { get; set; }
    public DateTime? ExpireDate { get; set; }
    public string? Description { get; set; }
    public bool IsVerified { get; set; }
    public FileAttachment? FileAttachments { get; set; }
    public virtual ConsumerPersonalInfo? ConsumerPersonalInfos { get; set; }

    public Guid? TenantId { get; set; }

    private ConsumerDocument() { }

    internal ConsumerDocument(
        Guid id,
        Guid consumerId,
        ConsumerDocumentType consumerDT,
        DateTime? issueDate,
        DateTime? expireDate,
        string? description,
        bool isVerified,
        FileAttachment? fileAttachments) : base(id)
    {
        ConsumerId = Check.NotNull(consumerId, nameof(consumerId));
        ConsumerDT = consumerDT;
        IssueDate = issueDate;
        ExpireDate = expireDate;
        SetDescription(description);
        IsVerified = false;
        FileAttachments = fileAttachments;
    }

    public ConsumerDocument ChangeDescription(string? description)
    {
        SetDescription(description);
        return this;
    }

    private void SetDescription(string? description)
    {
        if (description.IsNullOrWhiteSpace())
        {
            Description = null;
            return;
        }

        Description = Check.Length(
            description,
            nameof(description),
            ConsumerDocumentConsts.DescriptionMaxLength,
            0
        );
    }
}
