using Billing.ConsumerDocumentDetails;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Billing.ConsumerDocuments;

public class CreateConsumerDocumentDto
{
    [Required]
    public Guid ConsumerId { get; set; }

    public List<CreateConsumerDocumentDetailDto> DocumentDetails { get; set; } = new();
}
