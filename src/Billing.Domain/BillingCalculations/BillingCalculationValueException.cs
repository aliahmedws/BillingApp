using Volo.Abp;

namespace Billing.BillingCalculations;

public class BillingCalculationValueException : BusinessException
{
    public BillingCalculationValueException(string message) : base(BillingDomainErrorCodes.BillingCalculationError, message)
    {
        WithData("message", message);
    }
}

   