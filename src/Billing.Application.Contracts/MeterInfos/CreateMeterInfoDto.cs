using System;
using System.ComponentModel.DataAnnotations;

namespace Billing.MeterInfos;

public class CreateMeterInfoDto
{
    [Required]
    [StringLength(MeterInfoConsts.MaxMeterNoLength)]
    public string MeterNo { get; set; } = string.Empty;

    [Required]
    public MeterType MeterType { get; set; }

    [Required]
    public MeterCategory MeterCategory { get; set; }

    [Required]
    public MeterStatus MeterStatus { get; set; }

    [Required]
    public DateTime InstallationDate { get; set; }

    [Range((double)MeterInfoConsts.MinInitialReading, double.MaxValue)]
    public decimal InitialReading { get; set; }

    [Required]
    public Guid PhaseId { get; set; }

    [Required]
    public Guid PlotId { get; set; }

    [StringLength(MeterInfoConsts.MaxRemarksLength)]
    public string? Remarks { get; set; }
}
