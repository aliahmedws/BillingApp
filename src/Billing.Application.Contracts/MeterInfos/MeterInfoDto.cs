using System;
using Volo.Abp.Application.Dtos;

namespace Billing.MeterInfos;

public class MeterInfoDto : EntityDto<Guid>
{
    public string MeterNo { get; set; } = string.Empty;
    public MeterType MeterType { get; set; }
    public MeterCategory MeterCategory { get; set; }
    public MeterStatus MeterStatus { get; set; }
    public DateTime InstallationDate { get; set; }
    public decimal InitialReading { get; set; }
    public Guid PhaseId { get; set; }
    public string? PhaseName { get; set; }
    public Guid PlotId { get; set; }
    public string? PlotNo { get; set; }
    public string? Remarks { get; set; }
}
