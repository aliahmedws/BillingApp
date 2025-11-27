using Billing.GovtCharges;
using System.Threading.Tasks;
using Volo.Abp.Domain.Services;

namespace Billing.IescoCharges;

public class IescoChargeManager : DomainService
{
    private void ValidateChargeValue(decimal? value)
    {
        if (value.HasValue)
        {
            if (value < IescoChargeConsts.MinValue)
            {
                throw new IescoChargeValueNegException(value.Value);
            }

            if (value > IescoChargeConsts.MaxValue)
            {
                throw new IescoChargeValueLimitException(value.Value);
            }

            if (decimal.Round(value.Value, IescoChargeConsts.DecimalScale) != value.Value)
            {
                throw new IescoChargeDecimalScaleException(value.Value);
            }
        }
    }
    private void ValidateAllCharges(
        decimal? totalEnergyCharges,
        decimal? iescoFixCharges,
        decimal? serviceRent,
        decimal? varFpa,
        decimal? qtrTariffAdj,
        decimal? totalIescoCharges
        )
    {
        ValidateChargeValue(totalEnergyCharges);
        ValidateChargeValue(iescoFixCharges);
        ValidateChargeValue(serviceRent);
        ValidateChargeValue(varFpa);
        ValidateChargeValue(qtrTariffAdj);
        ValidateChargeValue(totalIescoCharges);
    }

    public async Task UpdateAsync(
        IescoCharge iescoCharge,
        decimal? totalEnergyCharges,
        decimal? iescoFixCharges,
        decimal? serviceRent,
        decimal? varFpa,
        decimal? qtrTariffAdj,
        decimal? totalIescoCharges
        )
    {
        ValidateAllCharges(
            totalEnergyCharges,
            iescoFixCharges,
            serviceRent,
            varFpa,
            qtrTariffAdj,
            totalIescoCharges
            );
        iescoCharge.TotalEnergyCharges = totalEnergyCharges ?? 0m;
        iescoCharge.IescoFixCharges = iescoFixCharges ?? 0m;
        iescoCharge.ServiceRent = serviceRent ?? 0m;
        iescoCharge.VarFpa = varFpa ?? 0m;
        iescoCharge.QtrTariffAdj = qtrTariffAdj ?? 0m;
        iescoCharge.TotalIescoCharges = totalIescoCharges ?? 0m;
    }
}
