using System;
using Volo.Abp;

namespace Billing.MaintenanceBills;

public class MaintenanceBillExpireDateMustBeAfterIssueDateException : BusinessException
{
    public MaintenanceBillExpireDateMustBeAfterIssueDateException(DateTime issueDate, DateTime expireDate)
           : base(BillingDomainErrorCodes.ExpireDateMustBeAfterIssueDate)
    {
        WithData("IssueDate", issueDate);
        WithData("ExpireDate", expireDate);
    }
}
