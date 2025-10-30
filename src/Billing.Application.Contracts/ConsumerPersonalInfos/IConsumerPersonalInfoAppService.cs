using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Billing.ConsumerPersonalInfos;

public interface IConsumerPersonalInfoAppService : IApplicationService
{
    Task<ConsumerPersonalInfoDto> GetAsync(Guid id);

    Task<PagedResultDto<ConsumerPersonalInfoDto>> GetListAsync(GetConsumerPersonalInfoListDto input);

    Task<ConsumerPersonalInfoDto> CreateAsync(CreateConsumerPersonalInfoDto input);

    Task UpdateAsync(Guid id, UpdateConsumerPersonalInfoDto input);

    Task DeleteAsync(Guid id);
}
