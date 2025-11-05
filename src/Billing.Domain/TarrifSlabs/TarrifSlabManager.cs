using System.Threading.Tasks;
using Volo.Abp.Domain.Services;

namespace Billing.TarrifSlabs;

public class TarrifSlabManager : DomainService
{
    private void ValidateRateValue(decimal? value, string fieldName)
    {
        if (value.HasValue)
        {
            if (value < 0)
            {
                throw new TarrifSlabValueLimitException($"{fieldName} cannot be negative.");
            }

            if (value < TarrifSlabConsts.MinValue || value > TarrifSlabConsts.MaxValue)
            {
                throw new TarrifSlabValueLimitException($"{fieldName} {TarrifSlabConsts.DecimalValidationMessage}");
            }

            if (decimal.Round(value.Value, TarrifSlabConsts.DecimalScale) != value.Value)
            {
                throw new TarrifSlabValueLimitException($"{fieldName} {TarrifSlabConsts.DecimalValidationMessage}");
            }
        }
    }

    private void ValidateAllRates(
        decimal rateRangeOne,
        decimal rateRangeTwo,
        decimal rateRangeThree,
        decimal rateRangeFour,
        decimal? rateRangeFive,
        decimal? rateRangeSix,
        decimal? rateRangeSeven,
        decimal? rateRangeEight
    )
    {
        ValidateRateValue(rateRangeOne, nameof(rateRangeOne));
        ValidateRateValue(rateRangeTwo, nameof(rateRangeTwo));
        ValidateRateValue(rateRangeThree, nameof(rateRangeThree));
        ValidateRateValue(rateRangeFour, nameof(rateRangeFour));
        ValidateRateValue(rateRangeFive, nameof(rateRangeFive));
        ValidateRateValue(rateRangeSix, nameof(rateRangeSix));
        ValidateRateValue(rateRangeSeven, nameof(rateRangeSeven));
        ValidateRateValue(rateRangeEight, nameof(rateRangeEight));
    }

    public async Task<TarrifSlab> CreateAsync(
        decimal rateRangeOne,
        decimal rateRangeTwo,
        decimal rateRangeThree,
        decimal rateRangeFour,
        decimal? rateRangeFive,
        decimal? rateRangeSix,
        decimal? rateRangeSeven,
        decimal? rateRangeEight
    )
    {
        ValidateAllRates(
            rateRangeOne,
            rateRangeTwo,
            rateRangeThree,
            rateRangeFour,
            rateRangeFive,
            rateRangeSix,
            rateRangeSeven,
            rateRangeEight
        );

        return new TarrifSlab(
            GuidGenerator.Create(),
            rateRangeOne ,
            rateRangeTwo,
            rateRangeThree,
            rateRangeFour,
            rateRangeFive ?? 0,
            rateRangeSix ?? 0,
            rateRangeSeven ?? 0,
            rateRangeEight ?? 0
        );
    }

    public async Task UpdateAsync(
        TarrifSlab tarrifSlab,
        decimal rateRangeOne,
        decimal rateRangeTwo,
        decimal rateRangeThree,
        decimal rateRangeFour,
        decimal? rateRangeFive,
        decimal? rateRangeSix,
        decimal? rateRangeSeven,
        decimal? rateRangeEight
    )
    {
        ValidateAllRates(
            rateRangeOne,
            rateRangeTwo,
            rateRangeThree,
            rateRangeFour,
            rateRangeFive,
            rateRangeSix,
            rateRangeSeven,
            rateRangeEight
        );

        tarrifSlab.RateRangeOne = rateRangeOne;
        tarrifSlab.RateRangeTwo = rateRangeTwo;
        tarrifSlab.RateRangeThree = rateRangeThree;
        tarrifSlab.RateRangeFour = rateRangeFour;
        tarrifSlab.RateRangeFive = rateRangeFive;
        tarrifSlab.RateRangeSix = rateRangeSix;
        tarrifSlab.RateRangeSeven = rateRangeSeven;
        tarrifSlab.RateRangeEight = rateRangeEight;
    }
}
