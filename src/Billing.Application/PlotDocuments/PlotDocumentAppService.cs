using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Services;

namespace Billing.PlotDocuments;

[RemoteService(isEnabled: false)]
public class PlotDocumentAppService : ApplicationService, IPlotDocumentAppService
{
    private readonly PlotDocumentManager _manager;

    public PlotDocumentAppService(PlotDocumentManager manager)
    {
        _manager = manager;
    }

    public async Task DeleteAsync(Guid id)
    {
        await _manager.DeleteAsync(id);
    }

    public async Task<PlotDocumentDto> UpdateAsync(Guid id, UpdatePlotDocumentDto input)
    {
        var data = await _manager.UpdateAsync(
            id,
            input.PlotInfoId,
            input.Description,
            input.DocumentNumber,
            input.IssueDate,
            input.ExpireDate,
            input.PlotDocumentType,
            input.IsVerified);

        return ObjectMapper.Map<PlotDocument, PlotDocumentDto>(data);
    }

    public async Task<PlotDocumentDto> UploadAsync(IFormFile file, CreatePlotDocumentDto input)
    {
        var result = await _manager.CreateAsync(
            input.PlotInfoId,
            input.Description,
            input.DocumentNumber,
            input.IssueDate,
            input.ExpireDate,
            input.PlotDocumentType,
            input.IsVerified,
            file);

        return ObjectMapper.Map<PlotDocument, PlotDocumentDto>(result);
    }
}
