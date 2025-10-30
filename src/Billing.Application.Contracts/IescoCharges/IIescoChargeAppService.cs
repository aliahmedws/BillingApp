using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Billing.IescoCharges;

public interface IIescoChargeAppService : IApplicationService
{ 
    public Task<IescoChargeDto> GetAsync(Guid id);
    public Task UpdateAsync(Guid id, UpdateIescoChargeDto input);
    public Task<PagedResultDto<IescoChargeDto>> GetListAsync();
}
