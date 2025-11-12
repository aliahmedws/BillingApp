using Billing.FileAttachments;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;

namespace Billing.MeterDocuments;

public class MeterDocumentManager : DomainService
{
    private readonly IRepository<MeterDocument, Guid> _repository;
    private readonly FileManager _fileManager;

    public MeterDocumentManager(IRepository<MeterDocument, Guid> repository, FileManager fileManager)
    {
        _fileManager = fileManager;
        _repository = repository;
    }
    public async Task<MeterDocument> CreateAsync(
    Guid meterInfoId,
    IFormFile file,
    MeterDocumentType documentType,
    string? description = null)
    {
        Check.NotNull(file, nameof(file));
        Check.NotNullOrWhiteSpace(file.FileName, nameof(file.FileName));
        Check.NotNull(meterInfoId, nameof(meterInfoId));

        var existing = await _repository.FirstOrDefaultAsync(d => d.MeterInfoId == meterInfoId && d.MeterDocumentType == documentType);

        if (existing != null)
        {
            if (!string.IsNullOrWhiteSpace(existing.FileAttachments?.BlobName))
            {
                await _fileManager.DeleteFileAsync(existing.FileAttachments);
            }

            await _repository.DeleteAsync(existing, autoSave: true);
        }

        using var stream = new MemoryStream();
        await file.CopyToAsync(stream);
        stream.Position = 0;
        var attachment = await _fileManager.SaveAsync(stream, file.FileName, "meter-documents");

        var document = new MeterDocument(
            GuidGenerator.Create(),
            meterInfoId,
            attachment,
            documentType,
            description
        );

        await _repository.InsertAsync(document, autoSave: true);
        return document;
    }

    public async Task DeleteAsync(Guid id)
    {
        Check.NotNull(id, nameof(id));

        var document = await _repository.FirstOrDefaultAsync(x => x.Id == id);

        if (document == null)
        {
            throw new MeterDocumentEmptyException();
        }

        if (document.FileAttachments != null && !string.IsNullOrWhiteSpace(document.FileAttachments.Path))
        {
            await _fileManager.DeleteFileAsync(document.FileAttachments);
        }

        await _repository.DeleteAsync(document, autoSave: true);
    }

    public async Task<MeterDocument> UpdateAsync(Guid id, MeterDocumentType meterDocumentType, string? meterDescription)
    {
        Check.NotNull(id, nameof(id));
        Check.NotNull(meterDocumentType, nameof(meterDocumentType));

        var document = await _repository.FirstOrDefaultAsync(x => x.Id == id);

        //if (document == null)
        //{
        //    throw new MeterDocumentEmptyException();
        //}

        document.MeterDocumentType = meterDocumentType;
        document.Description = meterDescription;

        await _repository.UpdateAsync(document, autoSave: true);
        return document;
    }
}
