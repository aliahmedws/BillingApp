using System;

namespace Billing.PlotInfos;

public class PlotInfoLookupDto
{
    public Guid Id { get; set; }
    public string PlotNo { get; set; } = string.Empty;
    public string BlockName { get; set; } = string.Empty;
    public string PlotSize { get; set; } = string.Empty;
    public Guid ConsumerId { get; set;  }
    public string ConsumerName { get; set; } = string.Empty;
}
