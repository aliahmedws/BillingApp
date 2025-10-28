using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace Billing.Blocks;

public interface IBlockRepository : IRepository<Block, Guid>
{
    Task<Block?> FindByNameAsync(string blockName, Guid phaseId);
    Task<Block?> FindByCodeAsync(string blockCode, Guid phaseId);
    Task<List<Block>> GetListAsync(
       int skipCount,
       int maxResultCount,
       string sorting,
       string? filter,
       string? blockCode,
       string? blockName,
       bool? isActive,
       string? description,
       Guid? phaseId);
    Task<long> GetCountAsync(
       string? filter,
       string? blockCode,
       string? blockName,
       bool? isActive,
       string? description,
       Guid? phaseId);
}
