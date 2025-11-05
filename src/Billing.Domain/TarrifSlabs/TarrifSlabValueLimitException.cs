using Volo.Abp;

namespace Billing.TarrifSlabs;

public class TarrifSlabValueLimitException : BusinessException
{
    public TarrifSlabValueLimitException(string message) : base(BillingDomainErrorCodes.TarrifSlabValueLimit, message)
    {
        WithData("message", message);
    }
}