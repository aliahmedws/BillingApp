using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Billing.TarrifSlabs;

public interface ITarrifSlabAppService : IApplicationService
{
    Task<TarrifSlabDto> GetAsync(Guid id);
    Task<PagedResultDto<TarrifSlabDto>> GetListAsync(GetTarrifSlabLIstDto input);
    Task<TarrifSlabDto> CreateAsync(CreateTarrifSlabDto input);
    Task UpdateAsync(Guid id, UpdateTarrifSlabDto input);
    Task DeleteAsync(Guid id);

}
