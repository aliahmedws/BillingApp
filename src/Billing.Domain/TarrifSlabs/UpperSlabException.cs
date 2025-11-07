using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;

namespace Billing.TarrifSlabs
{
    public class UpperSlabException : BusinessException
    {
        public UpperSlabException(decimal? upperSlab) : base(BillingDomainErrorCodes.upperSlabError)
        {
            WithData("upperSlab", upperSlab);
        }
    }
}
