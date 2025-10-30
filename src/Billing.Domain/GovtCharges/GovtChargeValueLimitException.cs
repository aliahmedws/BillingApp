using Volo.Abp;

namespace Billing.GovtCharges;

public class GovtChargeValueLimitException : BusinessException
{
    public GovtChargeValueLimitException(string message) : base(BillingDomainErrorCodes.GovtChargeValueLimitExceeded, message)
    {
    }
}