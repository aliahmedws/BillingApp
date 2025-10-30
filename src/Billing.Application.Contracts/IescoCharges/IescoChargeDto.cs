using System;
using Volo.Abp.Application.Dtos;

namespace Billing.IescoCharges;

public class IescoChargeDto : EntityDto<Guid>
{
    public decimal? TotalEnergyCharges { get; set; }   // will be total of tarrifslab x units consumed
    public decimal? IescoFixCharges { get; set; }
    public decimal? ServiceRent { get; set; }
    public decimal? VarFpa { get; set; }
    public decimal? QtrTariffAdj { get; set; }
    public decimal? TotalIescoCharges { get; set; }
}
