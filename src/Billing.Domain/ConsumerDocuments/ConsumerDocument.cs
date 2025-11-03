using Billing.ConsumerDocumentDetails;
using Billing.ConsumerPersonalInfos;
using System;
using System.Collections.Generic;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;

namespace Billing.ConsumerDocuments;

public class ConsumerDocument : FullAuditedAggregateRoot<Guid>
{
    public Guid ConsumerId { get; set; }
    public virtual ConsumerPersonalInfo Consumers { get; set; }

    public virtual ICollection<ConsumerDocumentDetail> ConsumerDocumentDetails { get; set; }

    private ConsumerDocument()
    {
        ConsumerDocumentDetails = new List<ConsumerDocumentDetail>();
    }

    internal ConsumerDocument(
        Guid id,
        Guid consumerId
        ) : base(id)
    {
        ConsumerId = Check.NotNull(consumerId, nameof(consumerId));
        ConsumerDocumentDetails = new List<ConsumerDocumentDetail>();
    }

}
