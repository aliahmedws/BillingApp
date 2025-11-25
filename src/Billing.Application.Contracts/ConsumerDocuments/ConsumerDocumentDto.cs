using Billing.FileAttachments;
using System;
using Volo.Abp.Application.Dtos;

namespace Billing.ConsumerDocuments;

public class ConsumerDocumentDto : EntityDto<Guid>
{
    public Guid ConsumerId { get; set; }
    public ConsumerDocumentType ConsumerDT { get; set; }
    public DateTime? IssueDate { get; set; }
    public DateTime? ExpireDate { get; set; }
    public string? Description { get; set; }
    public bool IsVerified { get; set; }
    public FileAttachmentDto FileAttachments { get; set; }
}
