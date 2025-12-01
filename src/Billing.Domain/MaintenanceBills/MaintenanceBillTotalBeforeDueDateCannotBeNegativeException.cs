using Volo.Abp;

namespace Billing.MaintenanceBills;

public class MaintenanceBillTotalBeforeDueDateCannotBeNegativeException : BusinessException
{
    public MaintenanceBillTotalBeforeDueDateCannotBeNegativeException(decimal paymentBeforeDueDate)
       : base(BillingDomainErrorCodes.MaintenanceBillTotalBeforeDueDateCannotBeNegative)
    {
        WithData("paymentBeforeDueDate", paymentBeforeDueDate);
    }
}
