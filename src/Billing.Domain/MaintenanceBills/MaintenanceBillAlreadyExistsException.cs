using System;
using Volo.Abp;

namespace Billing.MaintenanceBills;

public class MaintenanceBillAlreadyExistsException : BusinessException
{
    public MaintenanceBillAlreadyExistsException(string plotNo, DateTime billingMonth)
        : base(BillingDomainErrorCodes.MaintenanceBillAlreadyExists)
    {
        WithData("plotNo", plotNo);
        WithData("BillingMonth", billingMonth.ToString("yyyy-MM"));
    }
}
