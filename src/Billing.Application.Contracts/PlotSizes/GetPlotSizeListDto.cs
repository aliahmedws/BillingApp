using Volo.Abp.Application.Dtos;

namespace Billing.PlotSizes;

public class GetPlotSizeListDto : PagedAndSortedResultRequestDto
{
    public string? Filter { get; set; }
    public string? SizeName { get; set; }
    public bool? IsActive { get; set; }
    public PlotUnit? Unit { get; set; }
}
