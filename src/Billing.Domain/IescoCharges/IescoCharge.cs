using System;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace Billing.IescoCharges;

public class IescoCharge : FullAuditedAggregateRoot<Guid>, IMultiTenant
{
    public decimal? TotalEnergyCharges { get; set; } 
    public decimal? IescoFixCharges { get; set; }
    public decimal? ServiceRent { get; set; }
    public decimal? VarFpa { get; set; }
    public decimal? QtrTariffAdj { get; set; }
    public decimal? TotalIescoCharges { get; set; }
    public Guid? TenantId { get; set; }

    private IescoCharge()
    {
    }

    internal IescoCharge(
        Guid id,
        decimal? totalEnergyCharges,
        decimal? iescoFixCharges,
        decimal? serviceRent,
        decimal? varFpa,
        decimal? qtrTariffAdj,
        decimal? totalIescoCharges
        )
        : base(id)
    {
        TotalEnergyCharges = totalEnergyCharges ?? 0m;
        IescoFixCharges = iescoFixCharges ?? 0m;
        ServiceRent = serviceRent ?? 0m;
        VarFpa = varFpa ?? 0m;
        QtrTariffAdj = qtrTariffAdj ?? 0m;
        TotalIescoCharges = totalIescoCharges ?? 0m;
    }
}
