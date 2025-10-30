using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Billing.GovtCharges;

public interface IGovtChargeAppService : IApplicationService
{
    Task<GovtChargeDto> GetAsync(Guid id);
    Task<PagedResultDto<GovtChargeDto>> GetListAsync();
    Task UpdateAsync(Guid id, UpdateGovtChargeDto input);

}
