using Billing.ConsumerDocumentDetails;
using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace Billing.ConsumerDocuments;

public class ConsumerDocumentDto : EntityDto<Guid>
{
    public Guid ConsumerId { get; set; }
    public List<ConsumerDocumentDetailDto> ConsumerDocumentDetails { get; set; } = new();
}
