using Billing.PlotTypes;
using System;
using System.ComponentModel.DataAnnotations;

namespace Billing.PlotInfos;

public class UpdatePlotInfoDto
{
    [Required]
    [StringLength(PlotInfoConsts.MaxPlotNoLength)]
    public string PlotNo { get; set; } = string.Empty;

    [Required]
    public PlotType PlotType { get; set; }

    [Required]
    [StringLength(PlotInfoConsts.MaxStreetNoLength)]
    public string StreetNo { get; set; } = string.Empty;

    [Required]
    public Guid PlotSizeId { get; set; }

    [Required]
    public PlotStatus Status { get; set; }

    [Required]
    public Guid BlockId { get; set; }

    public Guid? ConsumerId { get; set; }

    [Required]
    public Guid PhaseId { get; set; }

    [StringLength(PlotInfoConsts.MaxRemarksLength)]
    public string? Remarks { get; set; }
}
