using Billing.ElectricityPaymentHistories;
using Billing.MaintenancePaymentHistories;
using Billing.MeterInfos;
using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace Billing.ElectricityBills;

public class ElectricityBill : FullAuditedAggregateRoot<Guid>, IMultiTenant
{
    public Guid? TenantId { get; set; }
    public Guid MeterInfoId { get; set; }
    public MeterInfo MeterInfos { get; set; }
    public DateTime BillingMonth { get; set; }
    public decimal PreviousReading { get; set; } = 0m;
    public decimal PresentReading { get; set; } = 0m;
    public decimal ConsumedUnits { get; set; } = 0m;
    public DateTime MeterReadingDate { get; set; }
    public DateTime IssueDate { get; set; }
    public DateTime DueDate { get; set; }
    public decimal CurrentMonthBill { get; set; } = 0m;
    public decimal BillAdjustment { get; set; } = 0m;
    public decimal AnyOtherCharges { get; set; } = 0m;
    public decimal PayableDueDateAmount { get; set; } = 0m;
    public decimal LPSurcharge { get; set; } = 0m;
    public decimal PayableAfterDueDateAmount { get; set; } = 0m;
    public ICollection<ElectricityPaymentHistory> ElectricityPaymentHistories { get; set; }



    private ElectricityBill() { }

    internal ElectricityBill(
        Guid id,
        Guid meterInfoId,
        decimal previousReading,
        decimal presentReading,
        decimal consumedUnits,
        DateTime billingMonth,
        DateTime meterReadingDate,
        DateTime issueDate,
        DateTime dueDate,
        decimal currentMonthBill,
        decimal billAdjustment,
        decimal anyOtherCharges,
        decimal payableDueDateAmount,
        decimal lpSurcharge,
        decimal payableAfterDueDateAmount
    ) : base(id)
    {
        MeterInfoId = meterInfoId;
        PreviousReading = previousReading;
        PresentReading = presentReading;
        ConsumedUnits = consumedUnits;
        BillingMonth = billingMonth;
        MeterReadingDate = meterReadingDate;
        IssueDate = issueDate;
        DueDate = dueDate;
        CurrentMonthBill = currentMonthBill;
        BillAdjustment = billAdjustment;
        AnyOtherCharges = anyOtherCharges;
        PayableDueDateAmount = payableDueDateAmount;
        LPSurcharge = lpSurcharge;
        PayableAfterDueDateAmount = payableAfterDueDateAmount;
    }


    internal void UpdateBillingCalculation(
        Guid meterInfoId,
        decimal previousReading,
        decimal presentReading,
        decimal consumedUnits,
        DateTime billingMonth,
        DateTime meterReadingDate,
        DateTime issueDate,
        DateTime dueDate,
        decimal currentMonthBill,
        decimal billAdjustment,  
        decimal anyOtherCharges,
        decimal payableDueDateAmount,
        decimal lpSurcharge,
        decimal payableAfterDueDateAmount
    )
    {
        MeterInfoId = meterInfoId;
        PreviousReading = previousReading;
        PresentReading = presentReading;
        ConsumedUnits = consumedUnits;
        BillingMonth = billingMonth;
        MeterReadingDate = meterReadingDate;
        IssueDate = issueDate;
        DueDate = dueDate;
        CurrentMonthBill = currentMonthBill;
        BillAdjustment = billAdjustment;
        AnyOtherCharges = anyOtherCharges;
        PayableDueDateAmount = payableDueDateAmount;
        LPSurcharge = lpSurcharge;
        PayableAfterDueDateAmount = payableAfterDueDateAmount;
    }
}
 