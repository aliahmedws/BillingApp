using System;

namespace Billing.SocietyCharges;

public class UpdateSocietyChargeDto
{
    public Guid PlotSizeId { get; set; }
    public decimal? SecurityCharges { get; set; }
    public decimal? MaintenanceCharges { get; set; }
    public decimal? WaterCharges { get; set; }
    public decimal? OtherCharges { get; set; }
    public decimal? TotalSocietyCharges { get; set; }
}
