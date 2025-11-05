using System;
using Volo.Abp;

namespace Billing.SocietyCharges;

public class SocietyChargeAlreadyExistException : BusinessException
{
    public SocietyChargeAlreadyExistException(Guid plotSizeId)
        : base(BillingDomainErrorCodes.SocietyChargeAlreadyExist)
    {
        WithData("plotSizeId", plotSizeId);
    }
}