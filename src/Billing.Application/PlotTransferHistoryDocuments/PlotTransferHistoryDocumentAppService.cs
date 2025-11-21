using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Services;

namespace Billing.PlotTransferHistoryDocuments;

[RemoteService(isEnabled: true)]
public class PlotTransferHistoryDocumentAppService : ApplicationService, IPlotTransferHistoryDocumentDtoAppService
{
    private readonly PlotTransferHistoryDocumentManager _manager;

    public PlotTransferHistoryDocumentAppService(PlotTransferHistoryDocumentManager manager)
    {
        _manager = manager;
    }

    public async Task DeleteAsync(Guid id)
    {
        await _manager.DeleteAsync(id);
    }

    public async Task<PlotTransferHistoryDocumentDto> UpdateAsync(Guid id, UpdatePlotTransferHistoryDocumentDto input)
    {
        var data = await _manager.UpdateAsync(
            id,
            input.PlotTransferHistoryId,
            input.PlotHistoryDT,
            input.RegistryNo,
            input.IssueDate,
            input.ExpireDate,
            input.Remarks,
            input.IsVerified);

        return ObjectMapper.Map<PlotTransferHistoryDocument, PlotTransferHistoryDocumentDto>(data);
    }

    public async Task<PlotTransferHistoryDocumentDto> UploadAsync(IFormFile file, CreatePlotTransferHistoryDocument input)
    {
        var result = await _manager.CreateAsync(
            input.PlotTransferHistoryId,
            input.PlotHistoryDT,
            input.RegistryNo,
            input.IssueDate,
            input.ExpireDate,
            input.Remarks,
            input.IsVerified,
            file);

        return ObjectMapper.Map<PlotTransferHistoryDocument, PlotTransferHistoryDocumentDto>(result);
    }
}
