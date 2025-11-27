using Volo.Abp;

namespace Billing.IescoCharges;

public class IescoChargeDecimalScaleException : BusinessException
{
    public IescoChargeDecimalScaleException(decimal? value) : base(BillingDomainErrorCodes.IescoChargeDecimalScale)
    {
        WithData("value", value);
        WithData("decimalScale", IescoChargeConsts.DecimalScale);
    }
}
