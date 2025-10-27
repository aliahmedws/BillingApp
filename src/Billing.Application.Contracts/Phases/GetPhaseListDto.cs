using Volo.Abp.Application.Dtos;

namespace Billing.Phases;

public class GetPhaseListDto : PagedAndSortedResultRequestDto
{
    public string? Filter { get; set; }
    public string? PhaseCode { get; set; }
    public string? PhaseName { get; set; }
    public bool? IsActive { get; set; }
    public string? Description { get; set; }
}
