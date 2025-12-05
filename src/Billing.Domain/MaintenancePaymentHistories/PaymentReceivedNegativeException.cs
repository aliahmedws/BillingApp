using Volo.Abp;

namespace Billing.MaintenancePaymentHistories;

public class PaymentReceivedNegativeException : BusinessException
{
    public PaymentReceivedNegativeException(decimal paymentReceived) 
        : base(BillingDomainErrorCodes.PaymentReceivedNegative)
    {
        WithData("paymentReceived", paymentReceived);
    }
}
