using Volo.Abp;

namespace Billing.Phases;

public class PhaseCodeAlreadyExistsException : BusinessException
{
    public PhaseCodeAlreadyExistsException(string? newCode) : base(BillingDomainErrorCodes.PhaseCodeAlreadyExists)
    {
        WithData("newCode", newCode!);
    }
}
