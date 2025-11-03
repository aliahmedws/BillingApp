using Billing.ConsumerDocumentDetails;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Domain.Services;

namespace Billing.ConsumerDocuments;

public class ConsumerDocumentManager : DomainService
{
    private readonly IConsumerDetailRepository _consumerDetailRepository;
    private readonly IConsumerDocumentDetailRepository _consumerDocumentDetailRepository;
    private readonly ConsumerDocumentDetailManager _consumerDocumentDetailManager;
    public ConsumerDocumentManager(
        IConsumerDetailRepository consumerDetailRepository,
        IConsumerDocumentDetailRepository consumerDocumentDetailRepository,
        ConsumerDocumentDetailManager consumerDocumentDetailManager)
    {
        _consumerDetailRepository = consumerDetailRepository;
        _consumerDocumentDetailRepository = consumerDocumentDetailRepository;
        _consumerDocumentDetailManager = consumerDocumentDetailManager;
    }

    public async Task<ConsumerDocument> CreateAsync(
        Guid consumerId,
        List<ConsumerDocumentDetail> documentDetails)
    {
        Check.NotNull(consumerId, nameof(consumerId));
        Check.NotNullOrEmpty(documentDetails, nameof(documentDetails));

        var consumerDocument = new ConsumerDocument(GuidGenerator.Create(), consumerId);

        consumerDocument.ConsumerDocumentDetails = documentDetails;
        return consumerDocument;
    }

    public async Task<ConsumerDocumentDetail> UpdateAsync(
        Guid documentDetailId,
        DocumentType documentType,
        DateTime? issueDate,
        DateTime? expireDate,
        string? description,
        bool isVerified,
        DateTime? verifiedDate,
        Guid? verifiedBy)
    {
        var detail = await _consumerDocumentDetailRepository.GetAsync(documentDetailId);
        if (detail == null)
            throw new UserFriendlyException("Consumer document detail not found");

        // Update metadata fields
        detail.DocumentType = documentType;
        detail.IssueDate = issueDate;
        detail.ExpireDate = expireDate;
        detail.Description = description;
        detail.IsVerified = isVerified;
        detail.VerifiedDate = verifiedDate;
        detail.VerifiedBy = verifiedBy;

        await _consumerDocumentDetailRepository.UpdateAsync(detail);
        return detail;
    }

}
