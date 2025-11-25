using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;

namespace Billing.TarrifSlabs;

public class UpperSlabAlreadyExist : BusinessException
{
    public UpperSlabAlreadyExist(decimal? upperSlab) : base(BillingDomainErrorCodes.UpperSlabAlreadyExists)
    {
        WithData("upperSlab", upperSlab);
    }
}
