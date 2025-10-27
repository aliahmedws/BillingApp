using Volo.Abp;

namespace Billing.Phases;

public class PhaseAlreadyExistsException : BusinessException
{
    public PhaseAlreadyExistsException(string phaseName) : base(BillingDomainErrorCodes.PhaseAlreadyExists)
    {
        WithData("phaseName", phaseName);
    }
}
