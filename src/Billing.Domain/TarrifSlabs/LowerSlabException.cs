using Volo.Abp;

namespace Billing.TarrifSlabs;

public class LowerSlabException : BusinessException
{
    public LowerSlabException(decimal lowerSlab) : base(BillingDomainErrorCodes.LowerSlabError)
    {
        WithData("lowerSlab", lowerSlab);
    }
} 