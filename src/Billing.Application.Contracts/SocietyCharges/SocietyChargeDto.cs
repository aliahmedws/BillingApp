using System;
using Volo.Abp.Application.Dtos;

namespace Billing.SocietyCharges;

public class SocietyChargeDto : EntityDto<Guid>
{
    public Guid PlotSizeId { get; set; }
    public string? SizeName { get; set; }
    public decimal? SecurityCharges { get; set; }
    public decimal? MaintenanceCharges { get; set; }
    public decimal? WaterCharges { get; set; }
    public decimal? OtherCharges { get; set; }
    public decimal? TotalSocietyCharges { get; set; }
}
