using Volo.Abp;

namespace Billing.ElectricityPaymentHistories;
    public class ElectricityPaymentAlreadyExistsException : BusinessException
    {
        public ElectricityPaymentAlreadyExistsException(string transactionId)
            : base(BillingDomainErrorCodes.ElectricityPaymentAlreadyExists)
        {
            WithData("transactionId", transactionId);
        }
    }
