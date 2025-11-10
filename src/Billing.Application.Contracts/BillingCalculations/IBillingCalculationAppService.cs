using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Billing.BillingCalculations;

public interface IBillingCalculationAppService : IApplicationService
{
    Task<BillingCalculationDto> GetAsync(Guid id);
    Task<PagedResultDto<BillingCalculationDto>> GetListAsync(GetBillingCalculationListDto input);
    Task<BillingCalculationDto> CreateAsync(CreateBillingCalculationDto input);
    Task UpdateAsync(Guid id, UpdateBillingCalculationDto input);
    Task DeleteAsync(Guid id);
}
