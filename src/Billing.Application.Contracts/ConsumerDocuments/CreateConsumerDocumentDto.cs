using System;

namespace Billing.ConsumerDocuments;

public class CreateConsumerDocumentDto
{
    public Guid ConsumerId { get; set; }
    public ConsumerDocumentType ConsumerDT { get; set; }
    public DateTime? IssueDate { get; set; }
    public DateTime? ExpireDate { get; set; }
    public string? Description { get; set; }
    public bool IsVerified { get; set; }
}
