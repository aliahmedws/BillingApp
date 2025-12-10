using Billing.MaintenancePaymentHistories;
using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Content;

namespace Billing.MaintenanceBills;

public interface IMaintenanceBillAppService : IApplicationService
{
    Task<MaintenanceBillDto> GetAsync(Guid id);

    Task<PagedResultDto<MaintenanceBillDto>> GetListAsync(GetMaintenanceBillListDto input);

    Task<MaintenanceBillDto> CreateAsync(CreateMaintenanceBillDto input);

    Task UpdateAsync(Guid id, UpdateMaintenanceBillDto input);

    Task DeleteAsync(Guid id);
    Task<GenerateMaintenanceBillsResultDto> GenerateAsync(GenerateMaintenanceBillsDto input);
    Task<IRemoteStreamContent> GetListAsExcelFileAsync(GetMaintenanceBillListDto input);
}
