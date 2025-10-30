using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Billing.SocietyCharges;

public interface ISocietyChargeAppService : IApplicationService
{
    Task<SocietyChargeDto> GetAsync(Guid id);
    Task<PagedResultDto<SocietyChargeDto>> GetListAsync(GetSocietyChargeListDto input);
    Task<SocietyChargeDto> CreateAsync(CreateSocietyChargeDto input);
    Task UpdateAsync(Guid id, UpdateSocietyChargeDto input);
    Task DeleteAsync(Guid id);
}
