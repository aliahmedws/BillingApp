using Volo.Abp;

namespace Billing.IescoCharges;

public class IescoChargeValueLimitException : BusinessException
{
    public IescoChargeValueLimitException(string message) : base(BillingDomainErrorCodes.IescoChargeValueLimitExceeded, message)
    {
    }
}