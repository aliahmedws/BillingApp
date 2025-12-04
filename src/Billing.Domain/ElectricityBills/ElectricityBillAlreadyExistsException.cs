using System;
using Volo.Abp;

namespace Billing.ElectricityBills;

public class ElectricityBillAlreadyExistsException : BusinessException
{
    public ElectricityBillAlreadyExistsException(string meterNo, DateTime billingMonth) : base(BillingDomainErrorCodes.ElectricityBillAlreadyExists)
    {
        WithData("meterNo", meterNo);
        WithData("billingMonth", billingMonth);
    }
}
