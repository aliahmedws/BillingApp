using Billing.FileAttachments;
using Billing.MeterDocuments;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;

namespace Billing.ConsumerDocuments;

public class ConsumerDocumentManager : DomainService
{
    private readonly IRepository<ConsumerDocument, Guid> _repository;
    private readonly FileManager _fileManager;

    public ConsumerDocumentManager(IRepository<ConsumerDocument, Guid> repository, FileManager fileManager)
    {
        _repository = repository;
        _fileManager = fileManager;
    }

    public async Task<ConsumerDocument> CreateAsync(
        Guid consumerId,
        ConsumerDocumentType consumerDT,
        DateTime? issueDate,
        DateTime? expireDate,
        string? description,
        bool isVerified,
        IFormFile file
        )   
    {
        Check.NotNull(consumerId, nameof(consumerId));
        Check.NotNull(consumerDT, nameof(consumerDT));
        //Check.NotNull(file, nameof(file));
        Check.NotNull(isVerified, nameof(isVerified));

        var existingDocument = await _repository.FirstOrDefaultAsync(x => x.ConsumerId == consumerId && x.ConsumerDT == consumerDT);

        if (existingDocument != null)
        {
            if (!string.IsNullOrWhiteSpace(existingDocument.FileAttachments?.Path))
            {
                await _fileManager.DeleteFileAsync(existingDocument.FileAttachments);
            }

            await _repository.DeleteAsync(existingDocument, autoSave: true);
        }

        using var stream = new MemoryStream();
        await file.CopyToAsync(stream);
        stream.Position = 0;

        var attachment = await _fileManager.SaveAsync(stream, file.FileName, "consumer-documents");

        var document = new ConsumerDocument(
            GuidGenerator.Create(),
            consumerId,
            consumerDT,
            issueDate,
            expireDate,
            description,
            isVerified,
            attachment);

        await _repository.InsertAsync(document, autoSave: true);
        return document;
    }

    public async Task DeleteAsync(Guid id)
    {
        Check.NotNull(id, nameof(id));

        var document = await _repository.FirstOrDefaultAsync(x => x.Id == id);

        if (document == null)
        {
            throw new DocumentEmptyException();
        }

        if (document.FileAttachments != null && !string.IsNullOrWhiteSpace(document.FileAttachments.Path))
        {
            await _fileManager.DeleteFileAsync(document.FileAttachments);
        }

        await _repository.DeleteAsync(document, autoSave: true);
    }

    public async Task<ConsumerDocument> UpdateAsync(
        Guid id,
        Guid consumerId,
        ConsumerDocumentType consumerDT,
        DateTime? issueDate,
        DateTime? expireDate,
        string? description,
        bool isVerified)
    {
        Check.NotNull(id, nameof(id));
        Check.NotNull(consumerId, nameof(consumerId));
        Check.NotNull(consumerDT, nameof(consumerDT));

        var document = await _repository.FirstOrDefaultAsync(x => x.Id == id);

        if (document == null) throw new DocumentEmptyException();

        document.ConsumerId = consumerId;
        document.ConsumerDT = consumerDT;
        document.IssueDate = issueDate;
        document.ExpireDate = expireDate;
        document.IsVerified = isVerified;
        document.ChangeDescription(description);


        await _repository.UpdateAsync(document, autoSave: true);
        return document;
    }
}
