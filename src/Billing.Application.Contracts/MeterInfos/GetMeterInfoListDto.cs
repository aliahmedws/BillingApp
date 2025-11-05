using System;
using Volo.Abp.Application.Dtos;

namespace Billing.MeterInfos;

public class GetMeterInfoListDto : PagedAndSortedResultRequestDto
{
    public string? Filter { get; set; }
    public string? MeterNo { get; set; }
    public MeterType? MeterType { get; set; }
    public MeterCategory? MeterCategory { get; set; }
    public MeterStatus? MeterStatus { get; set; }
    public DateTime? InstallationDate { get; set; }
    public Guid? PhaseId { get; set; }
    public Guid? PlotId { get; set; }
    public Guid? MeterOwnerId { get; set; }
}
