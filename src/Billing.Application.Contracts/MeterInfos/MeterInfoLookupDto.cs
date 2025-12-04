using System;
using Volo.Abp.Application.Dtos;

namespace Billing.MeterInfos;

public class MeterInfoLookupDto : EntityDto<Guid>
{
    public string MeterInfo { get; set; } = default!;
    public string OwnerName { get; set; } = default!;
}
