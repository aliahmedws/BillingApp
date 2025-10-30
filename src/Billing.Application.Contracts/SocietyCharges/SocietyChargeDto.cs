using System;
using Volo.Abp.Application.Dtos;

namespace Billing.SocietyCharges;

public class SocietyChargeDto : EntityDto<Guid>
{
    public decimal? SecurityCharges { get; set; }
    public decimal? MaintenanceCharges { get; set; }
    public decimal? WaterCharges { get; set; }
    public decimal? OtherCharges { get; set; }
    public decimal? TotalSocietyCharges { get; set; }
}
