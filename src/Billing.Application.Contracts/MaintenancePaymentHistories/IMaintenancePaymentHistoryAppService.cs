using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Billing.MaintenancePaymentHistories;
public interface IMaintenancePaymentHistoryAppService : IApplicationService
{
    Task<MaintenancePaymentHistoryDto> GetAsync(Guid id);
    Task<PagedResultDto<MaintenancePaymentHistoryDto>> GetListAsync(GetMaintenancePaymentHistoryListDto input);
    Task<MaintenancePaymentHistoryDto> CreateAsync(CreateMaintenancePaymentHistoryDto input);
    Task UpdateAsync(Guid id, UpdateMaintenancePaymentHistoryDto input);
    Task DeleteAsync(Guid id);
}
