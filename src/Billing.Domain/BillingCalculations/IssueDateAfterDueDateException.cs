using System;
using Volo.Abp;

namespace Billing.BillingCalculations;

public class IssueDateAfterDueDateException : BusinessException
{
    public IssueDateAfterDueDateException() : base(BillingDomainErrorCodes.BillingDateError)
    {
    }
}
