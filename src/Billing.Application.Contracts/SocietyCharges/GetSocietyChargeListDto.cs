using Volo.Abp.Application.Dtos;

namespace Billing.SocietyCharges;

public class GetSocietyChargeListDto : PagedAndSortedResultRequestDto
{
    public string? Filter { get; set; }
    public decimal? SecurityCharges { get; set; }
    public decimal? MaintenanceCharges { get; set; }
    public decimal? WaterCharges { get; set; }
    public decimal? OtherCharges { get; set; }
    public decimal? TotalSocietyCharges { get; set; }
}
