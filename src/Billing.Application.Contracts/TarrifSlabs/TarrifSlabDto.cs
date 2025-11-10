using System;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Application.Dtos;

namespace Billing.TarrifSlabs;

public class TarrifSlabDto : EntityDto<Guid>
{
    public decimal LowerSlab { get; set; }
    //[Required]
    //[Range(0, (double)TarrifSlabConsts.MaxValue)]
    public decimal? UpperSlab { get; set; }
    public decimal UnitPrice { get; set; }

}
