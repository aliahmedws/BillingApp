using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Billing.ConsumerDocuments;

public interface IConsumerDocumentAppService : IApplicationService
{
    Task<ConsumerDocumentDto> GetAsync(Guid id);

    Task<PagedResultDto<ConsumerDocumentDto>> GetListAsync(GetConsumerDocumentListDto input);

    Task<ConsumerDocumentDto> CreateAsync(CreateConsumerDocumentDto input);

    Task UpdateAsync(Guid id, CreateConsumerDocumentDto input);

    Task DeleteAsync(Guid id);
}
