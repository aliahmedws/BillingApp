using Volo.Abp;

namespace Billing.ElectricityPaymentHistories;

public class BillAmountRangeException : BusinessException
{
    public BillAmountRangeException(decimal paymentReceived) : base(BillingDomainErrorCodes.PaymentReceivedRangeError)
    {
        WithData("paymentReceived", paymentReceived);
        WithData("maxPaymentReceived", ElectricityPaymentHistoryConsts.MaxPaymentReceived);
    }
}
