using Volo.Abp;

namespace Billing.GovtCharges;

public class GovtChargeLessException : BusinessException
{
    public GovtChargeLessException(decimal? value) : base(BillingDomainErrorCodes.GovtChargeNegValue)
    {
        WithData("value", value);
        WithData("min", GovtChargeConsts.MinValue);
    }    
}
