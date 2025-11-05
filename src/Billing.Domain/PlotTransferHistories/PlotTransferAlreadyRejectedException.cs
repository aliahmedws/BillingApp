using Volo.Abp;

namespace Billing.PlotTransferHistories;

public class PlotTransferAlreadyRejectedException : BusinessException
{
    public PlotTransferAlreadyRejectedException(string registryNo)
        : base(BillingDomainErrorCodes.PlotTransferAlreadyRejected)
    {
        WithData("registryNo", registryNo);
    }
}
