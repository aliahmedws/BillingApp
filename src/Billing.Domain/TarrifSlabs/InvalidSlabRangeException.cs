using System;
using Volo.Abp;

namespace Billing.TarrifSlabs;

public class InvalidSlabRangeException : BusinessException
{
    public InvalidSlabRangeException(decimal previousUpper, decimal currentLower) : base("Billing:InvalidSlabRange")
    {
        WithData("previousUpper", previousUpper);
        WithData("currentLower", currentLower);
    }
}
