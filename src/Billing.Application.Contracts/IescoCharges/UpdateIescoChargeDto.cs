namespace Billing.IescoCharges;

public class UpdateIescoChargeDto
{
    public decimal? TotalEnergyCharges { get; set; }   // will be total of tarrifslab x units consumed
    public decimal? IescoFixCharges { get; set; }
    public decimal? ServiceRent { get; set; }
    public decimal? VarFpa { get; set; }
    public decimal? QtrTariffAdj { get; set; }
    public decimal? TotalIescoCharges { get; set; }
}
