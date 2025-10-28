using System;
using Volo.Abp.Application.Dtos;

namespace Billing.PlotSizes;

public class PlotSizeDto : EntityDto<Guid>
{
    public string SizeName { get; set; } = string.Empty;
    public decimal Area { get; set; }
    public PlotUnit Unit { get; set; }
    public decimal? Length { get; set; }
    public decimal? Width { get; set; }
    public string? Description { get; set; }
    public bool IsActive { get; set; }
}
