using Volo.Abp;

namespace Billing.GovtCharges;

public class GovtChargeValueExceedException : BusinessException
{
    public GovtChargeValueExceedException(decimal? value) : base(BillingDomainErrorCodes.GovtChargeValueExceed) 
    {
        WithData("value", value);
        WithData("max", GovtChargeConsts.MaxValue);
    }
}
