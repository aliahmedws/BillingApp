using System.Linq.Expressions;
using Volo.Abp;

namespace Billing.TarrifSlabs;

public class LowerSlabAlreadyExist : BusinessException
{
    public LowerSlabAlreadyExist(decimal lowerSlab) :   base(BillingDomainErrorCodes.LowerSlabAlreadyExists)
    {
        WithData("lowerSlab", lowerSlab);
    }
}
