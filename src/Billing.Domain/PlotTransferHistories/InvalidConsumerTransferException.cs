using System;
using Volo.Abp;

namespace Billing.PlotTransferHistories;

public class InvalidConsumerTransferException : BusinessException
{
    public InvalidConsumerTransferException(Guid consumerId) : base(BillingDomainErrorCodes.InvalidConsumerExists)
    {
        WithData("consumerId", consumerId);
    }
}
