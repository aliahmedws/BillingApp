using Billing.FileAttachments;
using Billing.MeterDocuments;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;

namespace Billing.PlotDocuments;

public class PlotDocumentManager : DomainService
{
    private readonly IRepository<PlotDocument, Guid> _repository;
    private readonly FileManager _fileManager;

    public PlotDocumentManager(IRepository<PlotDocument, Guid> repository, FileManager fileManager)
    {
        _repository = repository;
        _fileManager = fileManager;
    }

    public async Task<PlotDocument> CreateAsync(
        Guid plotInfoId,
        string? description,
        string? documentNumber,
        DateTime? issueDate,
        DateTime? expireDate,
        PlotDocumentType plotDocumentType,
        bool isVerified,
        IFormFile file)
    {
        Check.NotNull(plotInfoId, nameof(plotInfoId));
        Check.NotNull(plotDocumentType, nameof(plotDocumentType));
        Check.NotNull(isVerified, nameof(isVerified));
        Check.NotNull(file, nameof(file));

        var existingPlotDocument = await _repository.FirstOrDefaultAsync(d => d.PlotInfoId == plotInfoId && d.PlotDocumentType == plotDocumentType);

        if (existingPlotDocument != null)
        {
            if (!string.IsNullOrWhiteSpace(existingPlotDocument.FileAttachments?.Path))
            {
                await _fileManager.DeleteFileAsync(existingPlotDocument.FileAttachments);
            }

            await _repository.DeleteAsync(existingPlotDocument, autoSave: true);
        }

        using var stream = new MemoryStream();
        await file.CopyToAsync(stream);
        stream.Position = 0;
        var attachment = await _fileManager.SaveAsync(stream, file.FileName, "plot-documents");

        var document = new PlotDocument(
            GuidGenerator.Create(),
            plotInfoId,
            description,
            documentNumber,
            issueDate,
            expireDate,
            plotDocumentType,
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

    public async Task<PlotDocument> UpdateAsync(
        Guid id,
        Guid plotInfoId,
        string? description,
        string? documentNumber,
        DateTime? issueDate,
        DateTime? expireDate,
        PlotDocumentType plotDocumentType,
        bool isVerified)
    {
        Check.NotNull(id, nameof(id));
        Check.NotNull(plotInfoId, nameof(plotInfoId));
        Check.NotNull(plotDocumentType, nameof(plotDocumentType));

        var document = await _repository.FirstOrDefaultAsync(x => x.Id == id);

        if (document == null)
        {
            throw new DocumentEmptyException();
        }

        document.PlotInfoId = plotInfoId;
        document.ChangeDescription(description);
        document.ChangeDocumentNumber(documentNumber);
        document.IssueDate = issueDate;
        document.ExpireDate = expireDate;
        document.IsVerified = isVerified;

        await _repository.UpdateAsync(document, autoSave: true);
        return document;
    }

}
