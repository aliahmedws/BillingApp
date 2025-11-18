using Volo.Abp.Application.Dtos;

namespace Billing.TarrifSlabs;

public class GetTarrifSlabLIstDto : PagedAndSortedResultRequestDto
{
    public string? Filter { get; set; }
}
