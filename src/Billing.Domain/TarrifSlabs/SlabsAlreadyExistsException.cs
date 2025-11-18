using System.Linq.Expressions;
using Volo.Abp;

namespace Billing.TarrifSlabs;

public class SlabsAlreadyExistsException : BusinessException
{
    public SlabsAlreadyExistsException(decimal lowerSlab, decimal? upperSlab, decimal unitPrice) :
        base(BillingDomainErrorCodes.SlabAlreadyExists)
    {
        WithData("lowerSlab", lowerSlab);
        WithData("upperSlab", upperSlab!);
        WithData("unitPrice", unitPrice);
    }
}
