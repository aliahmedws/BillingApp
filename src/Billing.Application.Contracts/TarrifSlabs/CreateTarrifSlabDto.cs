using System;
using System.ComponentModel.DataAnnotations;

namespace Billing.TarrifSlabs;

public class CreateTarrifSlabDto
{
    [Required]
    [Range(0, (double)TarrifSlabConsts.MaxValue)]
    public decimal LowerSlab { get; set; }

    [Range(0, (double)TarrifSlabConsts.MaxValue)]
    public decimal? UpperSlab { get; set; }
    [Required]
    public decimal UnitPrice { get; set; }
}
