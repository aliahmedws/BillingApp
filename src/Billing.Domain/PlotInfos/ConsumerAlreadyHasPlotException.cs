using System;
using Volo.Abp;

namespace Billing.PlotInfos;

public class ConsumerAlreadyHasPlotException : BusinessException
{
    public ConsumerAlreadyHasPlotException(Guid consumerId) : base(BillingDomainErrorCodes.ConsumerAlreadyHasPlot)
    {
        WithData("consumerId", consumerId);
    }
}
