using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;

namespace Billing.TarrifSlabs;

public class UnitPriceAlreadyExist : BusinessException
{
    public UnitPriceAlreadyExist(decimal unitPrice) : base(BillingDomainErrorCodes.UnitPriceAlreadyExists)
    {
        WithData("unitPrice", unitPrice); 
    }
}
