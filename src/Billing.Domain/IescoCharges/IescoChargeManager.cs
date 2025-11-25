using System.Threading.Tasks;
using Volo.Abp.Domain.Services;

namespace Billing.IescoCharges;

public class IescoChargeManager : DomainService
{
    private void ValidateChargeValue(decimal? value, string fieldName)
    {
        if (value.HasValue)
        {
            if (value < 0)
            {
                throw new IescoChargeValueLimitException($"{fieldName} cannot be negative.");
            }

            if (value < IescoChargeConsts.MinValue || value > IescoChargeConsts.MaxValue)
            {
                throw new IescoChargeValueLimitException($"{fieldName} {IescoChargeConsts.DecimalValidationMessage}");
            }
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
        iescoCharge.TotalEnergyCharges = totalEnergyCharges ?? 0m;
        iescoCharge.IescoFixCharges = iescoFixCharges ?? 0m;
        iescoCharge.ServiceRent = serviceRent ?? 0m;
        iescoCharge.VarFpa = varFpa ?? 0m;
        iescoCharge.QtrTariffAdj = qtrTariffAdj ?? 0m;
        iescoCharge.TotalIescoCharges = totalIescoCharges ?? 0m;
    }
}
