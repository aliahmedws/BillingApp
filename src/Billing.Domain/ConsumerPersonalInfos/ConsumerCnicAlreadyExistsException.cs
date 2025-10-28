using Volo.Abp;

namespace Billing.ConsumerPersonalInfos;

public class ConsumerCnicAlreadyExistsException : BusinessException
{
    public ConsumerCnicAlreadyExistsException(string cnic) : base(BillingDomainErrorCodes.ConsumerCnicAlreadyExists)
    {
        WithData("cnic", cnic);
    }
}
