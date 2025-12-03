
using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using Volo.Abp;

namespace Billing.PlotSizes;

public class NegativeValueException : BusinessException
{
    public NegativeValueException(decimal value, string valueName) : base(BillingDomainErrorCodes.PlotNegValue)
    {
        WithData("value", value);
        WithData("valueName", valueName);

    }
}
