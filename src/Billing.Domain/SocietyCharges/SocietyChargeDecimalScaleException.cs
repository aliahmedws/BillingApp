using Volo.Abp;

namespace Billing.SocietyCharges;

public class SocietyChargeDecimalScaleException : BusinessException
{
    public SocietyChargeDecimalScaleException(decimal? value) : base(BillingDomainErrorCodes.SocietyChargeDecimalScale)
    {
        WithData("value", value);
        WithData("decimalScale", SocietyChargeConsts.DecimalScale);
    }
}
