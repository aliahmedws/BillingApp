using System;
using Volo.Abp.Application.Dtos;

namespace Billing.Blocks;

public class GetBlockListDto : PagedAndSortedResultRequestDto
{
    public string? Filter { get; set; }
    public string? BlockCode { get; set; }
    public string? BlockName { get; set; }
    public bool? IsActive { get; set; }
    public string? Description { get; set; }
    public Guid? PhaseId { get; set; }
}
