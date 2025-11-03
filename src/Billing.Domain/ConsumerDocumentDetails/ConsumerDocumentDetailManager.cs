using Billing.FileAttachments;
using System;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Domain.Services;

namespace Billing.ConsumerDocumentDetails;

public class ConsumerDocumentDetailManager : DomainService
{
    private readonly FileManager _fileManager;
    private readonly IConsumerDocumentDetailRepository _consumerDocumentDetailRepository;
    public ConsumerDocumentDetailManager(FileManager fileManager, IConsumerDocumentDetailRepository consumerDocumentDetailRepository)
    {
        _fileManager = fileManager;
        _consumerDocumentDetailRepository = consumerDocumentDetailRepository;
    }

    public async Task<ConsumerDocumentDetail> CreateAsync(
        Guid consumerDocumentId,
        DocumentType documentType,
        DateTime? issueDate,
        DateTime? expireDate,
        string? description,
        bool isVerified,
        DateTime? verifiedDate,
        Guid? verifiedBy,
        byte[] fileBytes,
        string fileName)
    {
        Check.NotNull(consumerDocumentId, nameof(consumerDocumentId));
        Check.NotNull(documentType, nameof(documentType));

        var consumerDetail = new ConsumerDocumentDetail(
            GuidGenerator.Create(),
            consumerDocumentId,
            documentType,
            issueDate,
            expireDate,
            description,
            isVerified,
            verifiedDate,
            verifiedBy);

        await UploadCompanyLogoAsync(fileBytes, fileName, consumerDetail);
        return consumerDetail;
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

    public async Task<ConsumerDocumentDetail> UploadCompanyLogoAsync(byte[] fileBytes, string fileName, ConsumerDocumentDetail consumerDocumentDetail)
    {
        if (consumerDocumentDetail.ConsumerDocumentFile != null)
        {
            await _fileManager.DeleteAsync(consumerDocumentDetail.ConsumerDocumentFile.BlobName);
        }

        var fileExtension = System.IO.Path.GetExtension(fileName);
        var (blobName, url) = await _fileManager.SaveAsync(fileBytes, fileExtension);

        consumerDocumentDetail.SetConsumerDocumentFile(fileName, blobName, url, fileBytes.Length);

        await _consumerDocumentDetailRepository.UpdateAsync(consumerDocumentDetail);
        return consumerDocumentDetail;
    }
}
