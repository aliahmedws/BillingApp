using Billing.FileAttachments;
using System;
using System.ComponentModel.DataAnnotations;

namespace Billing.ConsumerDocumentDetails;

public class CreateConsumerDocumentDetailDto
{
    [Required]
    public DocumentType DocumentType { get; set; }

    public DateTime? IssueDate { get; set; }
    public DateTime? ExpireDate { get; set; }
    public string? Description { get; set; }
    public bool IsVerified { get; set; }
    public DateTime? VerifiedDate { get; set; }
    public Guid? VerifiedBy { get; set; }

    public FileAttachmentDto? ConsumerDocumentFile { get; set; }
}
