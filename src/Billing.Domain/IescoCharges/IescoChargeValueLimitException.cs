using Volo.Abp;

namespace Billing.IescoCharges;

public class IescoChargeValueLimitException : BusinessException
{
    public IescoChargeValueLimitException(decimal? value) : base(BillingDomainErrorCodes.IescoChargeValueLimitExceeded)
    {
        WithData("value", value);
        WithData("max", IescoChargeConsts.MaxValue);

    }
}
