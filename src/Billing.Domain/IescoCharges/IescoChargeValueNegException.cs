using Volo.Abp;

namespace Billing.IescoCharges;

public class IescoChargeValueNegException : BusinessException
{
    public IescoChargeValueNegException(decimal? value) : base(BillingDomainErrorCodes.IescoChargeNegValue)
    {
        WithData("value", value);
        WithData("min", IescoChargeConsts.MinValue);
    }
}
