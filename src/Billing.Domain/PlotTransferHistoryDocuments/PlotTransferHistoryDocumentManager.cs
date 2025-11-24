using Billing.FileAttachments;
using Billing.MeterDocuments;
using Billing.PlotDocuments;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;

namespace Billing.PlotTransferHistoryDocuments;

public class PlotTransferHistoryDocumentManager : DomainService
{
    private readonly IRepository<PlotTransferHistoryDocument, Guid> _repository;
    private readonly FileManager _fileManager;

    public PlotTransferHistoryDocumentManager(
        IRepository<PlotTransferHistoryDocument, Guid> repository,
        FileManager fileManager)
    {
        _repository = repository;
        _fileManager = fileManager;
    }

    public async Task<PlotTransferHistoryDocument> CreateAsync(
        Guid plotTransferHistoryId,
        PlotHistoryDocumentType plotHistoryDT,
        DateTime? issueDate,
        DateTime? expireDate,
        string? remarks,
        bool isVerified,
        IFormFile file)
    {

        Check.NotNull(plotTransferHistoryId, nameof(plotTransferHistoryId));
        Check.NotNull(plotHistoryDT, nameof(plotHistoryDT));
        Check.NotNull(isVerified, nameof(isVerified));

        var existingDocument = await _repository.FirstOrDefaultAsync(d => d.PlotTransferHistoryId == plotTransferHistoryId && d.PlotHistoryDT == plotHistoryDT);

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

        var attachment = await _fileManager.SaveAsync(stream, file.FileName, "plot-history-documents");

        var document = new PlotTransferHistoryDocument(
            GuidGenerator.Create(),
            plotTransferHistoryId,
            plotHistoryDT,
            issueDate,
            expireDate,
            remarks,
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

    public async Task<PlotTransferHistoryDocument> UpdateAsync(
        Guid id,
        Guid plotTransferHistoryId,
        PlotHistoryDocumentType plotHistoryDT,
        DateTime? issueDate,
        DateTime? expireDate,
        string? remarks,
        bool isVerified)
    {
        Check.NotNull(id, nameof(id));
        Check.NotNull(plotTransferHistoryId, nameof(plotTransferHistoryId));
        Check.NotNull(plotHistoryDT, nameof(plotHistoryDT));

        var document = await _repository.FirstOrDefaultAsync(x => x.Id == id);

        if (document == null)
        {
            throw new DocumentEmptyException();
        }

        document.PlotTransferHistoryId = plotTransferHistoryId;
        document.PlotHistoryDT = plotHistoryDT;
        document.ExpireDate = expireDate;
        document.IssueDate = issueDate;
        document.ChangeRemarks(remarks);
        document.IsVerified = isVerified;

        await _repository.UpdateAsync(document, autoSave: true);
        return document;
    }
}
