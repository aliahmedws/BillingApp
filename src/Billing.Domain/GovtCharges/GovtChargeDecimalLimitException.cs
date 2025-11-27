using Billing.IescoCharges;
using Volo.Abp;

namespace Billing.GovtCharges;

public class GovtChargeDecimalLimitException : BusinessException
{
    public GovtChargeDecimalLimitException(decimal? value) : base(BillingDomainErrorCodes.GovtChargeDecimalLimitExceeded)
    {
        WithData("value", value);
        WithData("decimalScale", GovtChargeConsts.DecimalScale);
    }
}
