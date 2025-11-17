using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace Billing.ConsumerDocuments;

public interface IConsmerDocumentAppService : IApplicationService
{
    Task<ConsumerDocumentDto> UploadAsync(IFormFile file, CreateConsumerDocumentDto input);
    Task DeleteAsync(Guid id);
    Task<ConsumerDocumentDto> UpdateAsync(Guid id, UpdateConsumerDocumentDto input);
}
