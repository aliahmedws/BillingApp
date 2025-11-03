using System;

namespace Billing.PlotSizes;

public class PlotSizeLookupDto
{
    public Guid Id { get; set; }
    public string PlotSizeName { get; set; } = string.Empty;
}
