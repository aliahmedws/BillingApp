using Volo.Abp.Application.Dtos;

namespace Billing.TarrifSlabs;

public class GetTarrifSlabLIstDto : PagedAndSortedResultRequestDto
{
    public string? Filter { get; set; }
    public decimal? LowerSlab { get; set; }
    public decimal? UpperSlab { get; set; }
    public decimal? UnitPrice { get; set; }
}
