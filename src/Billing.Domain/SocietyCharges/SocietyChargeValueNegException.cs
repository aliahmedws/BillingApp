using Volo.Abp;

namespace Billing.SocietyCharges;

public class SocietyChargeValueNegException : BusinessException
{
    public SocietyChargeValueNegException(decimal? value) : base(BillingDomainErrorCodes.SocietyChargeNegValue)
    {
        WithData("value", value);
        WithData("min", SocietyChargeConsts.MinValue);
    }
}
