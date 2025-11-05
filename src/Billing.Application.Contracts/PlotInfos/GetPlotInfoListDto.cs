using System;
using Volo.Abp.Application.Dtos;

namespace Billing.PlotInfos;

public class GetPlotInfoListDto : PagedAndSortedResultRequestDto
{
    public string? Filter { get; set; }
    public string? PlotNo { get; set; }
    public string? StreetNo { get; set; }
    public Guid? BlockId { get; set; }
    public Guid? PhaseId { get; set; }
    public Guid? PlotSizeId { get; set; }
    public PlotStatus? Status { get; set; }
    public Guid? ConsumerId { get; set; }
}
