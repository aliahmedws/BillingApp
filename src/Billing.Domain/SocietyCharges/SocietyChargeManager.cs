using System.Threading.Tasks;
using Volo.Abp.Domain.Services;

namespace Billing.SocietyCharges;

public class SocietyChargeManager : DomainService
{
    private void ValidateChargeValue(decimal? value, string fieldName)
    {
        if (value.HasValue)
        {
            // ❌ 1. Check for negative values
            if (value < 0)
            {
                throw new SocietyChargeValueLimitException($"{fieldName} cannot be negative.");
            }
            // ❌ 2. Check for value range limits
            if (value < SocietyChargeConsts.MinValue || value > SocietyChargeConsts.MaxValue)
            {
                throw new SocietyChargeValueLimitException($"{fieldName} {SocietyChargeConsts.DecimalValidationMessage}");
            }
            // ❌ 3. Ensure max 3 decimal places
            if (decimal.Round(value.Value, SocietyChargeConsts.DecimalScale) != value.Value)
            {
                throw new SocietyChargeValueLimitException($"{fieldName} {SocietyChargeConsts.DecimalValidationMessage}");
            }

            // ❌ 4. Check charges already exists against same plotsize
            // This validation requires repository access, which is not implemented yet.
        }
    }

    private void ValidateAllCharges(
        decimal? securityCharges,
        decimal? maintenanceCharges,
        decimal? waterCharges,
        decimal? otherCharges,
        decimal? totalSocietyCharges
    )
    {
        ValidateChargeValue(securityCharges, nameof(securityCharges));
        ValidateChargeValue(maintenanceCharges, nameof(maintenanceCharges));
        ValidateChargeValue(waterCharges, nameof(waterCharges));
        ValidateChargeValue(otherCharges, nameof(otherCharges));
    }

    public async Task<SocietyCharge> CreateAsync(
        decimal? securityCharges,
        decimal? maintenanceCharges,
        decimal? waterCharges,
        decimal? otherCharges,
        decimal? totalSocietyCharges
    )
    {
        ValidateAllCharges(
            securityCharges,
            maintenanceCharges,
            waterCharges,
            otherCharges,
            totalSocietyCharges
        );
        var societyCharge = new SocietyCharge
        {
            SecurityCharges = securityCharges,
            MaintenanceCharges = maintenanceCharges,
            WaterCharges = waterCharges,
            OtherCharges = otherCharges,
            TotalSocietyCharges = totalSocietyCharges
        };
        return societyCharge;
    }

    public async Task UpdateAsync(
        SocietyCharge societyCharge,
        decimal? securityCharges,
        decimal? maintenanceCharges,
        decimal? waterCharges,
        decimal? otherCharges,
        decimal? totalSocietyCharges
    )
    {
        ValidateAllCharges(
            securityCharges,
            maintenanceCharges,
            waterCharges,
            otherCharges,
            totalSocietyCharges
        );
        societyCharge.SecurityCharges = securityCharges;
        societyCharge.MaintenanceCharges = maintenanceCharges;
        societyCharge.WaterCharges = waterCharges;
        societyCharge.OtherCharges = otherCharges;
        societyCharge.TotalSocietyCharges = totalSocietyCharges;
    }
}
