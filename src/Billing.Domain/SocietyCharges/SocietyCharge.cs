using System;
using Volo.Abp.Domain.Entities.Auditing;
using Billing.PlotSizes;

namespace Billing.SocietyCharges;

public class SocietyCharge : FullAuditedAggregateRoot<Guid>
{
    public Guid PlotSizeId { get;  set; }    // Foreign Key
    public PlotSize PlotSizes { get;  set; }  // Navigation property
    public decimal? SecurityCharges { get; private set; }
    public decimal? MaintenanceCharges { get; private set; }
    public decimal? WaterCharges { get; private set; }
    public decimal? OtherCharges { get; private set; }
    public decimal? TotalSocietyCharges { get; private set; }

    private SocietyCharge() { }

    internal SocietyCharge(
        Guid id,
        Guid plotSizeId,
        decimal? securityCharges = null,
        decimal? maintenanceCharges = null,
        decimal? waterCharges = null,
        decimal? otherCharges = null,
        decimal? totalSocietyCharges = null
        )
        : base(id)
    {
        PlotSizeId = plotSizeId;
        SecurityCharges = securityCharges;
        MaintenanceCharges = maintenanceCharges;
        WaterCharges = waterCharges;
        OtherCharges = otherCharges;
        TotalSocietyCharges = totalSocietyCharges;
    }

    internal void UpdateCharges(Guid plotSizeId, decimal? securityCharges, decimal? maintenanceCharges,
                                decimal? waterCharges, decimal? otherCharges, decimal? totalSocietyCharges)
    {
        //PlotSizeId = plotSizeId;
        SecurityCharges = securityCharges;
        MaintenanceCharges = maintenanceCharges;
        WaterCharges = waterCharges;
        OtherCharges = otherCharges;
        TotalSocietyCharges = totalSocietyCharges;
    }
}
