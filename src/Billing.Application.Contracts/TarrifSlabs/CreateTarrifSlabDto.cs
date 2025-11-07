using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Billing.TarrifSlabs;

public class CreateTarrifSlabDto
{
    [Required]
    public decimal LowerSlab { get; set; }

    public decimal? UpperSlab { get; set; }
    [Required]
    public decimal UnitPrice { get; set; }
}
