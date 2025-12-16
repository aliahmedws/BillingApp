using Billing.MaintenancePaymentHistories;
using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Content;

namespace Billing.ElectricityPaymentHistories;

public interface IElectricityPaymentHistoryAppService : IApplicationService
{
    Task<ElectricityPaymentHistoryDto> GetAsync(Guid id);
    Task<PagedResultDto<ElectricityPaymentHistoryDto>> GetListAsync(GetElectricityPaymentHistoryListDto input);
    Task<ElectricityPaymentHistoryDto> CreateAsync(CreateElectricityPaymentHistoryDto input);
    Task UpdateAsync(Guid id, UpdateElectricityPaymentHistoryDto input);
    Task DeleteAsync(Guid id);
    Task<IRemoteStreamContent> DownloadImportTemplateAsync();
    Task<ImportResultDto> ImportExcelFileAsync(IFormFile file);
}
