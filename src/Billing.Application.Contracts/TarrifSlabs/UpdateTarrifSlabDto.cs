
using System.ComponentModel.DataAnnotations;

namespace Billing.TarrifSlabs;

public class UpdateTarrifSlabDto
{
    [Range(0, (double)TarrifSlabConsts.MaxValue)]
    public decimal LowerSlab { get; set; }

    [Required]
    [Range(0, (double)TarrifSlabConsts.MaxValue)]
    public decimal? UpperSlab { get; set; }

    public decimal UnitPrice { get; set; }
}
