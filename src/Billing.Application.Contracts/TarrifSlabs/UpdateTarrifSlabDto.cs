
using System.ComponentModel.DataAnnotations;

namespace Billing.TarrifSlabs;

public class UpdateTarrifSlabDto
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
