using Volo.Abp;

namespace Billing.PlotTransferHistories;

public class PlotTransferAlreadyApprovedException : BusinessException
{
    public PlotTransferAlreadyApprovedException(string registryNo)
        : base(BillingDomainErrorCodes.PlotTransferAlreadyApproved)
    {
        WithData("registryNo", registryNo);
    }
}
