using Billing.FileAttachments;
using System;
using Volo.Abp.Application.Dtos;

namespace Billing.ConsumerDocumentDetails;

public class ConsumerDocumentDetailDto : EntityDto<Guid>
{
    public Guid ConsumerDocumentId { get; set; }
    public DocumentType DocumentType { get; set; }
    public DateTime? IssueDate { get; set; }
    public DateTime? ExpireDate { get; set; }
    public string? Description { get; set; }
    public bool IsVerified { get; set; }
    public DateTime? VerifiedDate { get; set; } 
    public Guid? VerifiedBy { get; set; }
    public FileAttachmentDto? ConsumerDocumentFile { get; set; }
}
