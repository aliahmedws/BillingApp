
using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using Volo.Abp;

namespace Billing.PlotSizes;

public class NegativeValueException : BusinessException
{
    public NegativeValueException(decimal value, string valueName) : base(BillingDomainErrorCodes.PlotNegValue)
    {
        //if (valueName.ToLower() == "length" && valueName.ToLower() == "width")
        //{
        //    valueName = "Length" + "Width";
        //}

        if (valueName.ToLower() == "length")
        {
            valueName = "Length";
        }
        else if (valueName.ToLower() == "width")
        {
            valueName = "Width";
        }
        
            WithData("value", value);
            WithData("valueName", valueName);

    }
}
