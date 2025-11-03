using Billing.ConsumerDocuments;
using Billing.FileAttachments;
using System;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;

namespace Billing.ConsumerDocumentDetails;

public class ConsumerDocumentDetail : FullAuditedAggregateRoot<Guid>
{
    public Guid ConsumerDocumentId { get; set; }
    public DocumentType DocumentType { get; set; }
    public DateTime? IssueDate { get; set; }
    public DateTime? ExpireDate { get; set; }
    public string? Description { get; set; }
    public bool IsVerified { get; set; }
    public DateTime? VerifiedDate { get; set; }
    public Guid? VerifiedBy { get; set; }
    public virtual ConsumerDocument ConsumerDocument { get; set; }
    public FileAttachment ConsumerDocumentFile { get; set; }

    private ConsumerDocumentDetail() { }

    public ConsumerDocumentDetail(
        Guid id,
        Guid consumerDocumentId,
        DocumentType documentType,
        DateTime? issueDate,
        DateTime? expireDate,
        string? description,
        bool isVerified,
        DateTime? verifiedDate,
        Guid? verifiedBy) : base(id)
    {
        ConsumerDocumentId = Check.NotNull(consumerDocumentId, nameof(consumerDocumentId));
        DocumentType = documentType;
        IssueDate = issueDate;
        ExpireDate = expireDate;
        Description = Check.NotNullOrWhiteSpace(description, nameof(description), maxLength: ConsumerDocumentDetailConsts.DescriptionMaxLength);
        IsVerified = false;
        VerifiedDate = verifiedDate;
        VerifiedBy = verifiedBy;
    }

    public ConsumerDocumentDetail ChangeDescription(string? description)
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
            ConsumerDocumentDetailConsts.DescriptionMaxLength,
            0
        );
    }

    internal ConsumerDocumentDetail SetCosnumerDocumentFile(FileAttachment fileAttachment)
    {
        ConsumerDocumentFile = Check.NotNull(fileAttachment, nameof(fileAttachment));
        return this;
    }

    internal ConsumerDocumentDetail SetConsumerDocumentFile(string name, string blobName, string path, long sizeInBytes)
    {
        ConsumerDocumentFile = new FileAttachment(name, blobName, path, sizeInBytes);
        return this;
    }

    internal ConsumerDocumentDetail RemoveConsumerDocumentFile()
    {
        ConsumerDocumentFile = null;
        return this;
    }

}
