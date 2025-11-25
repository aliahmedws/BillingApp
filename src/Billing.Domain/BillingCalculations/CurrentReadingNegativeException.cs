using System;
using Volo.Abp;

namespace Billing.BillingCalculations;

[Serializable]
public class CurrentReadingNegativeException : BusinessException
{
    public CurrentReadingNegativeException()
        : base(BillingDomainErrorCodes.BillingNegativeValueError)
    {
    }
}
