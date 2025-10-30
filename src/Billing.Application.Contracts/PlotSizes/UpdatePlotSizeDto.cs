using System.ComponentModel.DataAnnotations;

namespace Billing.PlotSizes;

public class UpdatePlotSizeDto
{
    [Required]
    [StringLength(PlotSizeConsts.MaxSizeNameLength)]
    public string SizeName { get; set; } = string.Empty;

    [Required]
    public decimal Area { get; set; }

    [Required]
    public PlotUnit Unit { get; set; }

    public decimal? Length { get; set; }
    public decimal? Width { get; set; }

    [StringLength(PlotSizeConsts.MaxDescriptionLength)]
    public string? Description { get; set; }

    public bool IsActive { get; set; } = true;
}
