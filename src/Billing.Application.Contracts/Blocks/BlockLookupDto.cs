using System;

namespace Billing.Blocks;

public class BlockLookupDto
{
    public Guid Id { get; set; }
    public string BlockCode { get; set; } = string.Empty;
    public string BlockName { get; set; } = string.Empty;
}
