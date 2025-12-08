using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Billing.MeterInfos;

public interface IMeterInfoAppService : IApplicationService
{
    Task<MeterInfoDto> GetAsync(Guid id);

    Task<PagedResultDto<MeterInfoDto>> GetListAsync(GetMeterInfoListDto input);

    Task<MeterInfoDto> CreateAsync(CreateMeterInfoDto input);

    Task UpdateAsync(Guid id, UpdateMeterInfoDto input);

    Task DeleteAsync(Guid id);
    Task<List<MeterInfoLookupDto>> GetMeterInfoLookupAsync();
}
