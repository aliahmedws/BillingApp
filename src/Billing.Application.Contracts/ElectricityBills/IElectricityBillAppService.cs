using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Billing.ElectricityBills;

public interface IElectricityBillAppService : IApplicationService
{
    Task<ElectricityBillDto> GetAsync(Guid id);

    Task<PagedResultDto<ElectricityBillDto>> GetListAsync(GetElectricityBillListDto input);

    Task<ElectricityBillDto> CreateAsync(CreateElectricityBillDto input);

    Task UpdateAsync(Guid id, UpdateElectricityBillDto input);

    Task DeleteAsync(Guid id);
    Task<decimal> CalculateBillAsync(int units);
}
