using Volo.Abp;

namespace Billing.SocietyCharges;

public class SocietyChargeValueLimitException : BusinessException
{
    public SocietyChargeValueLimitException(decimal? value) : base(BillingDomainErrorCodes.SocietyChargeValueLimitExceeded)
    {
        WithData("value", value);
        WithData("max", SocietyChargeConsts.MaxValue);
    }
}
