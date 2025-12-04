using Volo.Abp;

namespace Billing.TarrifSlabs;

public class UpperSlabException : BusinessException
{
    public UpperSlabException(decimal? upperSlab) : base(BillingDomainErrorCodes.upperSlabError)
    {
        WithData("upperSlab", upperSlab!);
    }
}
