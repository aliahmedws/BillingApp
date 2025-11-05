using Billing.ConsumerDocumentDetails;
using System;
using Volo.Abp.Application.Dtos;

namespace Billing.ConsumerDocuments;

public class GetConsumerDocumentListDto : PagedAndSortedResultRequestDto
{
    public string? Filter { get; set; }
    public Guid? ConsumerId { get; set; }
    public DocumentType? DocumentType { get; set; }
    public bool? IsVerified { get; set; }
    public DateTime? IssueDate { get; set; }
    public DateTime? ExpireDate { get; set; }
}
