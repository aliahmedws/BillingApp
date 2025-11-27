using System;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Domain.Services;

namespace Billing.SocietyCharges;

public class SocietyChargeManager : DomainService
{
    private readonly ISocietyChargeRepository _societyChargeRepository;

    public SocietyChargeManager(ISocietyChargeRepository societyChargeRepository)
    {
        _societyChargeRepository = societyChargeRepository;
    }
    private void ValidateChargeValue(decimal? value)
    {
        if (value.HasValue)
        {
            if (value < SocietyChargeConsts.MinValue)
            {
                throw new SocietyChargeValueNegException(value.Value);
            }

            if (value > SocietyChargeConsts.MaxValue)
            {
                throw new SocietyChargeValueLimitException(value.Value);
            }

            if (decimal.Round(value.Value, SocietyChargeConsts.DecimalScale) != value.Value)
            {
                throw new SocietyChargeDecimalScaleException(value.Value);
            }
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
        ValidateChargeValue(securityCharges);
        ValidateChargeValue(maintenanceCharges);
        ValidateChargeValue(waterCharges);
        ValidateChargeValue(otherCharges);
        ValidateChargeValue(totalSocietyCharges);
    }

    public async Task<SocietyCharge> CreateAsync(
        Guid plotSizeId,
        decimal? securityCharges,
        decimal? maintenanceCharges,
        decimal? waterCharges,
        decimal? otherCharges,
        decimal? totalSocietyCharges)
    {
        Check.NotNull(plotSizeId, nameof(plotSizeId));
        ValidateAllCharges(securityCharges, maintenanceCharges, waterCharges, otherCharges, totalSocietyCharges);

        var existingSocietyCharge = await _societyChargeRepository.FindByNameAsync(plotSizeId);
        if (existingSocietyCharge != null)
        {
            throw new SocietyChargeAlreadyExistException(plotSizeId);
        }

        var societyCharge = new SocietyCharge(
            GuidGenerator.Create(),
            plotSizeId,
            securityCharges,
            maintenanceCharges,
            waterCharges,
            otherCharges,
            totalSocietyCharges
        );

        return societyCharge;
    }

    public async Task UpdateAsync(
        SocietyCharge societyCharge,
        Guid plotSizeId,
        decimal? securityCharges,
        decimal? maintenanceCharges,
        decimal? waterCharges,
        decimal? otherCharges,
        decimal? totalSocietyCharges
        )
    {
        Check.NotNull(societyCharge, nameof(societyCharge));

        ValidateAllCharges(securityCharges, maintenanceCharges, waterCharges, otherCharges, totalSocietyCharges);

        societyCharge.UpdateCharges(
            plotSizeId,
            securityCharges,
            maintenanceCharges,
            waterCharges,
            otherCharges,
            totalSocietyCharges
        );

    }
}
