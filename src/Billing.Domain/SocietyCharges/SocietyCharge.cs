using System;
using Volo.Abp.Domain.Entities.Auditing;

namespace Billing.SocietyCharges;

public class SocietyCharge : FullAuditedAggregateRoot<Guid>
{
    public decimal? SecurityCharges { get; set; } = 0;
    public decimal? MaintenanceCharges { get; set; } = 0;
    public decimal? WaterCharges { get; set; } = 0;
    public decimal? OtherCharges { get; set; } = 0;
    public decimal? TotalSocietyCharges { get; set; } = 0;
}
