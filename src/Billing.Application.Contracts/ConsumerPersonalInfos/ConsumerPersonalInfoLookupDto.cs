using System;

namespace Billing.ConsumerPersonalInfos;

public class ConsumerPersonalInfoLookupDto
{
    public Guid Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string lastName { get; set; } = string.Empty;
}
