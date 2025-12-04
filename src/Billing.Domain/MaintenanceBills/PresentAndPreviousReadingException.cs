using Volo.Abp;

namespace Billing.MaintenanceBills;

public class PresentAndPreviousReadingException : BusinessException
{
    public PresentAndPreviousReadingException(decimal previousReading, decimal presentReading) : base(BillingDomainErrorCodes.PresentAndPreviousReading)
    {
        WithData("previousReading", previousReading);
        WithData("presentReading", presentReading);

    }
}
