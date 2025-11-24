using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;

namespace Billing.TarrifSlabs;

public class UnitPriceLessException : BusinessException
{
    public UnitPriceLessException(decimal unitPrice)
        : base(BillingDomainErrorCodes.UnitPriceLessError)
    {
        WithData("unitPrice", unitPrice);
    }
}
