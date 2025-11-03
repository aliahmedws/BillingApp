using Billing.ConsumerDocumentDetails;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;

namespace Billing.ConsumerDocuments;

[RemoteService(isEnabled: false)]
public class ConsumerDocumentAppService : BillingAppService, IConsumerDocumentAppService
{
    private readonly IConsumerDetailRepository _consumerDetailRepository;
    private readonly IConsumerDocumentDetailRepository _consumerDocumentDetailRepository;
    private readonly ConsumerDocumentManager _consumerDocumentManager;
    private readonly ConsumerDocumentDetailManager _consumerDocumentDetailManager;

    public ConsumerDocumentAppService(
        IConsumerDetailRepository consumerDetailRepository,
        IConsumerDocumentDetailRepository consumerDocumentDetailRepository,
        ConsumerDocumentManager consumerDocumentManager,
        ConsumerDocumentDetailManager consumerDocumentDetailManager)
    {
        _consumerDetailRepository = consumerDetailRepository;
        _consumerDocumentDetailRepository = consumerDocumentDetailRepository;
        _consumerDocumentManager = consumerDocumentManager;
        _consumerDocumentDetailManager = consumerDocumentDetailManager;
    }

    #region Create
    //[Authorize(BillingPermissions.ConsumerDocuments.Create)]
    public async Task<ConsumerDocumentDto> CreateAsync(CreateConsumerDocumentDto input)
    {
        Check.NotNull(input, nameof(input));
        Check.NotNull(input.ConsumerId, nameof(input.ConsumerId));

        var documentDetails = new List<ConsumerDocumentDetail>();

        foreach (var detailDto in input.DocumentDetails)
        {
            byte[] fileBytes = Array.Empty<byte>();
            string fileName = string.Empty;

            if (detailDto.ConsumerDocumentFile != null)
            {
                fileBytes = detailDto.ConsumerDocumentFile.FileBytes ?? Array.Empty<byte>();
                fileName = detailDto.ConsumerDocumentFile.Name ?? string.Empty;
            }

            var detail = await _consumerDocumentDetailManager.CreateAsync(
                Guid.Empty, // linked later
                detailDto.DocumentType,
                detailDto.IssueDate,
                detailDto.ExpireDate,
                detailDto.Description,
                detailDto.IsVerified,
                detailDto.VerifiedDate,
                detailDto.VerifiedBy,
                fileBytes,
                fileName
            );

            documentDetails.Add(detail);
        }

        var consumerDocument = await _consumerDocumentManager.CreateAsync(input.ConsumerId, documentDetails);
        await _consumerDetailRepository.InsertAsync(consumerDocument, autoSave: true);

        return ObjectMapper.Map<ConsumerDocument, ConsumerDocumentDto>(consumerDocument);
    }
    #endregion

    #region Get (Single)
    //[Authorize(BillingPermissions.ConsumerDocuments.Default)]
    public async Task<ConsumerDocumentDto> GetAsync(Guid id)
    {
        var document = await _consumerDetailRepository
            .GetAsync(x => x.Id == id, includeDetails: true);

        if (document == null)
            throw new UserFriendlyException("Consumer document not found.");

        return ObjectMapper.Map<ConsumerDocument, ConsumerDocumentDto>(document);
    }
    #endregion

    #region Get List
    //[Authorize(BillingPermissions.ConsumerDocuments.Default)]
    public async Task<PagedResultDto<ConsumerDocumentDto>> GetListAsync(GetConsumerDocumentListDto input)
    {
        if (input.Sorting.IsNullOrWhiteSpace())
        {
            input.Sorting = nameof(ConsumerDocument.CreationTime) + " DESC";
        }

        var query = await _consumerDetailRepository.GetQueryableAsync();

        //query = query
        //    .WhereIf(input.ConsumerId.HasValue, x => x.ConsumerId == input.ConsumerId)
        //    .WhereIf(!input.Filter.IsNullOrWhiteSpace(),
        //        x => x.ConsumerDocumentDetails.Any(d =>
        //            d.Description!.ToLower().Contains(input.Filter.ToLower()) ||
        //            d.DocumentType.ToString().ToLower().Contains(input.Filter.ToLower())))
        //    .OrderBy(input.Sorting);

        var totalCount = await AsyncExecuter.CountAsync(query);
        var items = await AsyncExecuter.ToListAsync(
            query.Skip(input.SkipCount).Take(input.MaxResultCount)
        );

        return new PagedResultDto<ConsumerDocumentDto>(
            totalCount,
            ObjectMapper.Map<List<ConsumerDocument>, List<ConsumerDocumentDto>>(items)
        );
    }
    #endregion

    #region Update
    //[Authorize(BillingPermissions.ConsumerDocuments.Edit)]
    public async Task UpdateAsync(Guid id, CreateConsumerDocumentDto input)
    {
        var existing = await _consumerDetailRepository.GetAsync(id, includeDetails: true);
        if (existing == null)
            throw new UserFriendlyException("Consumer document not found.");

        // Remove or update existing details
        foreach (var detailDto in input.DocumentDetails)
        {
            var existingDetail = existing.ConsumerDocumentDetails
                .FirstOrDefault(d => d.DocumentType == detailDto.DocumentType);

            if (existingDetail == null)
            {
                // Add new document detail
                byte[] fileBytes = detailDto.ConsumerDocumentFile?.FileBytes ?? Array.Empty<byte>();
                string fileName = detailDto.ConsumerDocumentFile?.Name ?? string.Empty;

                var newDetail = await _consumerDocumentDetailManager.CreateAsync(
                    existing.Id,
                    detailDto.DocumentType,
                    detailDto.IssueDate,
                    detailDto.ExpireDate,
                    detailDto.Description,
                    detailDto.IsVerified,
                    detailDto.VerifiedDate,
                    detailDto.VerifiedBy,
                    fileBytes,
                    fileName
                );

                existing.ConsumerDocumentDetails.Add(newDetail);
            }
            else
            {
                // Update existing detail
                await _consumerDocumentDetailManager.UpdateAsync(
                    existingDetail.Id,
                    detailDto.DocumentType,
                    detailDto.IssueDate,
                    detailDto.ExpireDate,
                    detailDto.Description,
                    detailDto.IsVerified,
                    detailDto.VerifiedDate,
                    detailDto.VerifiedBy
                );

                // Update file if new provided
                if (detailDto.ConsumerDocumentFile?.FileBytes != null)
                {
                    await _consumerDocumentDetailManager.UploadCompanyLogoAsync(
                        detailDto.ConsumerDocumentFile.FileBytes,
                        detailDto.ConsumerDocumentFile.Name,
                        existingDetail
                    );
                }
            }
        }

        await _consumerDetailRepository.UpdateAsync(existing, autoSave: true);
    }
    #endregion

    #region Delete
    //[Authorize(BillingPermissions.ConsumerDocuments.Delete)]
    public async Task DeleteAsync(Guid id)
    {
        var document = await _consumerDetailRepository.GetAsync(id);
        if (document == null)
            throw new UserFriendlyException("Consumer document not found.");

        await _consumerDetailRepository.DeleteAsync(document);
    }
    #endregion
}