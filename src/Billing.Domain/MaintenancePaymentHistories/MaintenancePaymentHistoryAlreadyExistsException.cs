using Volo.Abp;

namespace Billing.MaintenancePaymentHistories; 

public class MaintenancePaymentHistoryAlreadyExistsException : BusinessException
{
    public MaintenancePaymentHistoryAlreadyExistsException(string transactionId) : 
        base(BillingDomainErrorCodes.MaintenancePaymentHistoryAlreadyExists) 
    {
        WithData("transactionId", transactionId);
    }
}
