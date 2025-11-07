using System;
using System.Threading.Tasks;
using Volo.Abp.Domain.Services;
using Volo.Abp.MultiTenancy;
using Volo.Abp.TenantManagement;

namespace Billing.TarrifSlabs;

public class TarrifSlabManager : DomainService
{
    private void ValidateRateValue(decimal lowerSlab, decimal? upperSlab, decimal unitPrice)
    {

        if (lowerSlab < TarrifSlabConsts.MinValue || lowerSlab > TarrifSlabConsts.MaxValue)
            throw new LowerSlabException(lowerSlab);

        if (upperSlab.HasValue && upperSlab <= lowerSlab) throw new UpperSlabException(upperSlab);

        if (unitPrice < 0) throw new UnitPriceException(unitPrice);
    }

    private void ValidateAllRates(decimal lowerSlab, decimal? upperSlab, decimal unitPrice)
    {
        ValidateRateValue(lowerSlab, upperSlab, unitPrice);
    }

    public async Task<TarrifSlab> CreateAsync(decimal lowerSlab, decimal? upperSlab, decimal unitPrice)
    {
        ValidateAllRates(lowerSlab, upperSlab, unitPrice);

        return new TarrifSlab(GuidGenerator.Create(),
            lowerSlab,
            upperSlab,
            unitPrice
        );
    }

    public async Task UpdateAsync(TarrifSlab tarrifSlab, decimal lowerSlab, decimal? upperSlab,decimal unitPrice)
    {
        ValidateRateValue(lowerSlab, upperSlab, unitPrice);

        tarrifSlab.LowerSlab = lowerSlab;
        tarrifSlab.UpperSlab = upperSlab;
        tarrifSlab.UnitPrice = unitPrice;
    }
}
