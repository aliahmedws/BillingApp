using Billing.MeterInfos;
using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Services;

namespace Billing.MeterDocuments;


[RemoteService(isEnabled: false)]
public class MeterDocumentAppService : ApplicationService, IMeterDocumentAppService
{
    private readonly MeterDocumentManager _manager;

    public MeterDocumentAppService(MeterDocumentManager manager)
    {
        _manager = manager;
    }

    public async Task<MeterDocumentDto> UploadAsync(IFormFile file, CreateMeterDocumentDto input)
    {
        var result = await _manager.CreateAsync(input.MeterId, file, input.Type, input.Description);
        return ObjectMapper.Map<MeterDocument, MeterDocumentDto>(result);
    }

    public async Task DeleteAsync(Guid id)
    {
        await _manager.DeleteAsync(id);
    }

    public async Task<MeterDocumentDto> UpdateAsync(Guid id, UpdateMeterDocumentDto input)
    {
        var data = await _manager.UpdateAsync(
            id,
            input.Type,
            input.Description);

        return ObjectMapper.Map<MeterDocument, MeterDocumentDto>(data);
    }

}
