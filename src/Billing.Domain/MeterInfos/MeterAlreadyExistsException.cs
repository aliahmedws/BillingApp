using Volo.Abp;

namespace Billing.MeterInfos;

public class MeterAlreadyExistsException : BusinessException
{
    public MeterAlreadyExistsException(string meterNo) : base(BillingDomainErrorCodes.MeterAlreadyExists)
    {
        WithData("meterNo", meterNo);
    }
}
