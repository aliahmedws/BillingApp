using Volo.Abp;

namespace Billing.MaintenanceBills;

public class NegativeAmountNotAllowedException : BusinessException
{
    public NegativeAmountNotAllowedException() : base(BillingDomainErrorCodes.NegativeAmountNotAllowed)
    {

    }
}
