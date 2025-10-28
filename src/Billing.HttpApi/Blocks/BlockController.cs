using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc;

namespace Billing.Blocks;

[RemoteService(IsEnabled = true)]
[ControllerName("Blocks")]
[Area("app")]
[Route("api/app/blocks")]
public class BlockController : AbpController, IBlockAppService
{
    private readonly IBlockAppService _blockAppService;

    public BlockController(IBlockAppService blockAppService)
    {
        _blockAppService = blockAppService;
    }

    [HttpGet("{id}")]
    public async Task<BlockDto> GetAsync(Guid id)
    {
        return await _blockAppService.GetAsync(id);
    }

    [HttpGet]
    public async Task<PagedResultDto<BlockDto>> GetListAsync(GetBlockListDto input)
    {
        return await _blockAppService.GetListAsync(input);
    }

    [HttpPost]
    public async Task<BlockDto> CreateAsync(CreateBlockDto input)
    {
        return await _blockAppService.CreateAsync(input);
    }

    [HttpPut("{id}")]
    public async Task UpdateAsync(Guid id, UpdateBlockDto input)
    {
        await _blockAppService.UpdateAsync(id, input);
    }

    [HttpDelete("{id}")]
    public async Task DeleteAsync(Guid id)
    {
        await _blockAppService.DeleteAsync(id);
    }
}
