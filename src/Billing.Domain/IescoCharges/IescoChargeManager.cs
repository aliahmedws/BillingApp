using System.Threading.Tasks;
using Volo.Abp.Domain.Services;

namespace Billing.IescoCharges;

public class IescoChargeManager : DomainService
{
    private void ValidateChargeValue(decimal? value, string fieldName)
    {
        if (value.HasValue)
        {
            // ❌ 1. Check for negative values
            if (value < 0)
            {
                throw new IescoChargeValueLimitException($"{fieldName} cannot be negative.");
            }

            if (value < IescoChargeConsts.MinValue || value > IescoChargeConsts.MaxValue)
            {
                throw new IescoChargeValueLimitException($"{fieldName} {IescoChargeConsts.DecimalValidationMessage}");
            }
            // Ensure max 3 decimal places
            if (decimal.Round(value.Value, IescoChargeConsts.DecimalScale) != value.Value)
            {
                throw new IescoChargeValueLimitException($"{fieldName} {IescoChargeConsts.DecimalValidationMessage}");
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
        ValidateChargeValue(totalEnergyCharges, nameof(totalEnergyCharges));
        ValidateChargeValue(iescoFixCharges, nameof(iescoFixCharges));
        ValidateChargeValue(serviceRent, nameof(serviceRent));
        ValidateChargeValue(varFpa, nameof(varFpa));
        ValidateChargeValue(qtrTariffAdj, nameof(qtrTariffAdj));
        ValidateChargeValue(totalIescoCharges, nameof(totalIescoCharges));
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
        iescoCharge.TotalEnergyCharges = totalEnergyCharges;
        iescoCharge.IescoFixCharges = iescoFixCharges;
        iescoCharge.ServiceRent = serviceRent;
        iescoCharge.VarFpa = varFpa;
        iescoCharge.QtrTariffAdj = qtrTariffAdj;
        iescoCharge.TotalIescoCharges = totalIescoCharges;
    }
}
