using System;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Domain.Services;

namespace Billing.Blocks;

public class BlockManager : DomainService
{
    private readonly IBlockRepository _blockRepository;

    public BlockManager(IBlockRepository blockRepository)
    {
        _blockRepository = blockRepository;
    }

    public async Task<Block> CreateAsync(
        string blockCode,
        string blockName,
        Guid phaseId,
        string? description = null,
        bool isActive = true)
    {
        Check.NotNullOrWhiteSpace(blockName, nameof(blockName));
        Check.NotNullOrWhiteSpace(blockCode, nameof(blockCode));

        var existingBlock = await _blockRepository.FindByNameAsync(blockName, phaseId);
        if (existingBlock != null)
        {
            throw new BlockAlreadyExistException(blockName);
        }


        var existingBlockByCode = await _blockRepository.FindByCodeAsync(blockCode, phaseId);
        if (existingBlockByCode != null)
        {
            throw new BlockCodeAlreadyExistException(blockCode);
        }

        return new Block(
            GuidGenerator.Create(),
            blockCode,
            blockName,
            phaseId,
            description,
            isActive
        );
    }

    public async Task UpdateAsync(
        Block block,
        string? blockCode,
        string blockName,
        Guid phaseId,
        string? description,
        bool isActive)
    {
        Check.NotNull(block, nameof(block));
        Check.NotNullOrWhiteSpace(blockName, nameof(blockName));

        var existingBlock = await _blockRepository.FindByNameAsync(blockName, phaseId);
        if (existingBlock != null && existingBlock.Id != block.Id)
        {
            throw new BlockAlreadyExistException(blockName);
        }

        var existingBlockByCode = await _blockRepository.FindByCodeAsync(blockCode, phaseId);
        if (existingBlockByCode != null)
        {
            throw new BlockCodeAlreadyExistException(blockCode);
        }


        block
            .ChangeBlockCode(blockCode)
            .ChangeBlockName(blockName)
            .ChangeDescription(description)
            .SetPhase(phaseId)
            .SetActiveStatus(isActive);
    }
}
