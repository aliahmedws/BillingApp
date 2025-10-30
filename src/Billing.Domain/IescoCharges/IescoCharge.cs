using System;
using Volo.Abp.Domain.Entities.Auditing;

namespace Billing.IescoCharges;

public class IescoCharge : FullAuditedAggregateRoot<Guid>
{
    public decimal? TotalEnergyCharges { get; set; }   // will be total of tarrifslab x units consumed
    public decimal? IescoFixCharges { get; set; }
    public decimal? ServiceRent { get; set; }
    public decimal? VarFpa { get; set; }
    public decimal? QtrTariffAdj { get; set; }
    public decimal? TotalIescoCharges { get; set; }

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
        TotalEnergyCharges = totalEnergyCharges;
        IescoFixCharges = iescoFixCharges;
        ServiceRent = serviceRent;
        VarFpa = varFpa;
        QtrTariffAdj = qtrTariffAdj;
        TotalIescoCharges = totalIescoCharges;
    }
}
