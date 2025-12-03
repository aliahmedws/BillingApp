using System;
using Volo.Abp;

namespace Billing.MaintenanceBills;

public class MaintenanceBillAlreadyExistsException : BusinessException
{
    public MaintenanceBillAlreadyExistsException(Guid plotInfoId, DateTime billingMonth)
        : base(BillingDomainErrorCodes.MaintenanceBillAlreadyExists)
    {
        WithData("PlotInfoId", plotInfoId);
        WithData("BillingMonth", billingMonth.ToString("yyyy-MM"));
    }
}
