using Volo.Abp;

namespace Billing.PlotTransferHistories;

public class PlotTransferRegistryAlreadyExistsException : BusinessException
{
    public PlotTransferRegistryAlreadyExistsException(string registryNo) : base(BillingDomainErrorCodes.PlotTransferRegistryAlreadyExists)
    {
        WithData("registryNo", registryNo);
    }
}
