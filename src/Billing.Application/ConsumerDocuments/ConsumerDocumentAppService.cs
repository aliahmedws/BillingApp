using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Services;

namespace Billing.ConsumerDocuments;

[RemoteService(isEnabled: false)]
public class ConsumerDocumentAppService : ApplicationService, IConsumerDocumentAppService
{
    private readonly ConsumerDocumentManager _manager;

    public ConsumerDocumentAppService(ConsumerDocumentManager manager)
    {
        _manager = manager;
    }
    public async Task DeleteAsync(Guid id)
    {
        await _manager.DeleteAsync(id);
    }

    public async Task<ConsumerDocumentDto> UpdateAsync(Guid id, UpdateConsumerDocumentDto input)
    {
        var data = await _manager.UpdateAsync(
            id,
            input.ConsumerId,
            input.ConsumerDT,
            input.IssueDate,
            input.ExpireDate,
            input.Description,
            input.IsVerified);

        return ObjectMapper.Map<ConsumerDocument, ConsumerDocumentDto>(data);
    }

    public async Task<ConsumerDocumentDto> UploadAsync(IFormFile file, CreateConsumerDocumentDto input)
    {
        var result = await _manager.CreateAsync(
            input.ConsumerId,
            input.ConsumerDT,
            input.IssueDate,
            input.ExpireDate,
            input.Description,
            input.IsVerified,
            file);

        return ObjectMapper.Map<ConsumerDocument, ConsumerDocumentDto>(result);
    }
}
