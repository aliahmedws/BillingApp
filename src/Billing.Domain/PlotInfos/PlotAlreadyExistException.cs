using Volo.Abp;

namespace Billing.PlotInfos;

public class PlotAlreadyExistException : BusinessException
{
    public PlotAlreadyExistException(string plotNo) : base(BillingDomainErrorCodes.PlotAlreadyExists)
    {
        WithData("plotNo", plotNo);
    }
}
