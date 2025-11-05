using System;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Application.Dtos;

namespace Billing.TarrifSlabs;

public class TarrifSlabDto : EntityDto<Guid>
{
    [Required]
    public decimal RateRangeOne { get; set; }
    [Required]
    public decimal RateRangeTwo { get; set; }
    [Required]
    public decimal RateRangeThree { get; set; }
    [Required]
    public decimal RateRangeFour { get; set; }
    public decimal? RateRangeFive { get; set; }
    public decimal? RateRangeSix { get; set; }
    public decimal? RateRangeSeven { get; set; }
    public decimal? RateRangeEight { get; set; }
}
