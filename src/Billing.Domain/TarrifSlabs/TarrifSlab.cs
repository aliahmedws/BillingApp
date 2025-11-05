using System;
using Volo.Abp.Domain.Entities.Auditing;

namespace Billing.TarrifSlabs;

public class TarrifSlab : FullAuditedAggregateRoot<Guid>
{
    public decimal RateRangeOne { get; set; }
    public decimal RateRangeTwo { get; set; }
    public decimal RateRangeThree { get; set; }
    public decimal RateRangeFour { get; set; }
    public decimal? RateRangeFive { get; set; }
    public decimal? RateRangeSix { get; set; }
    public decimal? RateRangeSeven { get; set; }
    public decimal? RateRangeEight { get; set; }

    public TarrifSlab ()
    {
    }

    internal TarrifSlab (
        Guid id,
        decimal rateRangeOne,
        decimal rateRangeTwo,
        decimal rateRangeThree,
        decimal rateRangeFour,
        decimal? rateRangeFive,
        decimal? rateRangeSix,
        decimal? rateRangeSeven,
        decimal? rateRangeEight
        ) : base ( id )
    {
        RateRangeOne = rateRangeOne;
        RateRangeTwo = rateRangeTwo;
        RateRangeThree = rateRangeThree;
        RateRangeFour = rateRangeFour;
        RateRangeFive = rateRangeFive;
        RateRangeSix = rateRangeSix;
        RateRangeSeven = rateRangeSeven;
        RateRangeEight = rateRangeEight;
    }
}
