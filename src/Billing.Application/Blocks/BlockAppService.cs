using Billing.Permissions;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;

namespace Billing.Blocks;

[RemoteService(isEnabled: false)]
[Authorize(BillingPermissions.Blocks.Default)]
public class BlockAppService : BillingAppService, IBlockAppService
{
    private readonly IBlockRepository _blockRepository;
    private readonly BlockManager _blockManager;

    public BlockAppService(IBlockRepository blockRepository, BlockManager blockManager)
    {
        _blockRepository = blockRepository;
        _blockManager = blockManager;
    }

    [Authorize(BillingPermissions.Blocks.Create)]
    public async Task<BlockDto> CreateAsync(CreateBlockDto input)
    {
        var block = await _blockManager.CreateAsync(
            input.BlockCode,
            input.BlockName,
            input.PhaseId,
            input.Description,
            input.IsActive
        );

        await _blockRepository.InsertAsync(block);

        return ObjectMapper.Map<Block, BlockDto>(block);
    }

    [Authorize(BillingPermissions.Blocks.Delete)]
    public async Task DeleteAsync(Guid id)
    {
        await _blockRepository.DeleteAsync(id);
    }

    public async Task<BlockDto> GetAsync(Guid id)
    {
        var block = await _blockRepository.GetAsync(id);
        return ObjectMapper.Map<Block, BlockDto>(block);
    }

    public async Task<PagedResultDto<BlockDto>> GetListAsync(GetBlockListDto input)
    {
        if (input.Sorting.IsNullOrWhiteSpace())
        {
            input.Sorting = nameof(Block.BlockName);
        }

        var items = await _blockRepository.GetListAsync(
            input.SkipCount,
            input.MaxResultCount,
            input.Sorting!,
            input.Filter,
            input.BlockCode,
            input.BlockName,
            input.IsActive,
            input.Description,
            input.PhaseId
        );

        var totalCount = await _blockRepository.GetCountAsync(
            input.Filter,
            input.BlockCode,
            input.BlockName,
            input.IsActive,
            input.Description,
            input.PhaseId
        );

        return new PagedResultDto<BlockDto>(
            totalCount,
            ObjectMapper.Map<List<Block>, List<BlockDto>>(items)
        );
    }

    [Authorize(BillingPermissions.Blocks.Edit)]
    public async Task UpdateAsync(Guid id, UpdateBlockDto input)
    {
        var block = await _blockRepository.GetAsync(id);

        await _blockManager.UpdateAsync(
            block,
            input.BlockCode,
            input.BlockName,
            input.PhaseId,
            input.Description,
            input.IsActive
        );

        await _blockRepository.UpdateAsync(block);
    }
}
