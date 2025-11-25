using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Billing.Blocks;

public interface IBlockAppService : IApplicationService
{
    Task<BlockDto> GetAsync(Guid id);

    Task<PagedResultDto<BlockDto>> GetListAsync(GetBlockListDto input);

    Task<BlockDto> CreateAsync(CreateBlockDto input);

    Task UpdateAsync(Guid id, UpdateBlockDto input);

    Task DeleteAsync(Guid id);
    Task<List<BlockLookupDto>> GetBlockLookupAsync();
    Task<List<BlockLookupDto>> GetBlocksByPhaseIdAsync(Guid phaseId);
}
