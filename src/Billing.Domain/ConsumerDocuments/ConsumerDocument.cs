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

    public ConsumerDocument AddDetail(
            DocumentType documentType,
            DateTime? issueDate,
            DateTime? expireDate,
            string? description,
            string? fileFrontPath,
            string? fileBackPath,
            string? filePath,
            bool isVerfied,
            DateTime? verfiedDate,
            Guid? verfiedBy
            )
    {

        var detail = new ConsumerDocumentDetail(
            Guid.NewGuid(),
            Id,
            documentType,
            issueDate,
            expireDate,
            fileFrontPath,
            fileBackPath,
            filePath,
            description,
            isVerfied,
            verfiedDate,
            verfiedBy);

        ConsumerDocumentDetails.Add(detail);
        return this;
    }

}
