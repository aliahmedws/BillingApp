using System;
using Volo.Abp;

namespace Billing.SocietyCharges;

public class SocietyChargeValueLimitException : BusinessException
{
    public SocietyChargeValueLimitException(string message) : base(BillingDomainErrorCodes.SocietyChargeValueLimitExceeded, message)
    {
    }
}