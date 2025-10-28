using System;
using Volo.Abp.Application.Dtos;

namespace Billing.Blocks;

public class BlockDto : EntityDto<Guid>
{
    public string BlockCode { get; set; } = string.Empty;
    public string BlockName { get; set; } = string.Empty;
    public Guid PhaseId { get; set; }
    public string? PhaseName { get; set; } 
    public string? Description { get; set; }
    public bool IsActive { get; set; }
}
