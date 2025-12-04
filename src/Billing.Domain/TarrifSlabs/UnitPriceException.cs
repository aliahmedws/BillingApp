using Volo.Abp;

namespace Billing.TarrifSlabs;

public class UnitPriceException : BusinessException
{
    public UnitPriceException(decimal unitPrice) : base(BillingDomainErrorCodes.UnitPriceError)
    {
        WithData("unitPrice", unitPrice);
    }
}
