using Volo.Abp;

namespace Billing.ConsumerPersonalInfos;

public class ConsumerPhoneAlreadyExistsException : BusinessException
{
    public ConsumerPhoneAlreadyExistsException(string phone) : base(BillingDomainErrorCodes.ConsumerPhoneAlreadyExists)
    {
        WithData("phone", phone);
    }
}
