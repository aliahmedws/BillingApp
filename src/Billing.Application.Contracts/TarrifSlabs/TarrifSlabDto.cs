using System;
using Volo.Abp.Application.Dtos;

namespace Billing.TarrifSlabs;

public class TarrifSlabDto : EntityDto<Guid>
{
    public decimal LowerSlab { get; set; }
    public decimal? UpperSlab { get; set; }
    public decimal UnitPrice { get; set; }

}
