using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;

namespace Billing.TarrifSlabs
{
    public class UnitPriceException : BusinessException
    {
        public UnitPriceException(decimal unitPrice) : base(BillingDomainErrorCodes.UnitPriceError)
        {
            WithData("unitPrice", unitPrice);
        }
    }
}
