using AppBilling.MonthNames;
using Billing.MeterInfos;
using System;
using Volo.Abp.Domain.Entities.Auditing;

namespace Billing.BillingCalculations;

public class BillingCalculation : FullAuditedAggregateRoot<Guid>
{
    public Guid MeterInfoId { get; set; }
    public MeterInfo MeterInfos { get; set; }
    public decimal CurrentReading { get; set; }
    public decimal? PreviousReading { get; set; }
    public decimal ConsumedUnits { get; set; }
    public DateTime MeterReadingDate { get; set; }
    public decimal UnitsAmount { get; set; }
    public MonthName BillingMonth { get; set; }
    public decimal? TotalGovtCharges { get; set; }
    public decimal? TotalIescoCharges { get; set; }
    public DateTime? IssueDate { get; set; }
    public DateTime? DueDate{ get; set; }
    public decimal? AmountBeforeDueDate { get; set; }
    public decimal? AmountAfterDueDate { get; set; }
     

    private BillingCalculation () { }

    internal BillingCalculation(
        Guid id,
        Guid meterInfoId,
        decimal currentReading,
        decimal? previousReading,
        decimal consumedUnits,
        MonthName billingMonth,
        decimal? totalGovtCharges,
        decimal? totalIescoCharges,
        DateTime? issueDate,
        DateTime? dueDate,
        decimal? amountBeforeDueDate,
        decimal? amountAfterDueDate
        ) : base (id)
    {
        MeterInfoId = meterInfoId;
        CurrentReading = currentReading;
        PreviousReading = previousReading;
        ConsumedUnits = consumedUnits;
        BillingMonth = billingMonth;
        TotalGovtCharges = totalGovtCharges;
        TotalIescoCharges = totalIescoCharges;
        AmountBeforeDueDate = amountBeforeDueDate;
        AmountAfterDueDate = amountAfterDueDate;
    }

    internal void UpdateBillingCalculation(
        Guid meterInfoId,
        decimal currentReading,
        decimal? previousReading,
        decimal consumedUnits,
        MonthName billingMonth,
        decimal? totalGovtCharges,
        decimal? totalIescoCharges,
        DateTime? issueDate,
        DateTime? dueDate,
        decimal? amountBeforeDueDate,
        decimal? amountAfterDueDate
        )
    {
        MeterInfoId = meterInfoId;
        CurrentReading = currentReading;
        PreviousReading = previousReading;
        ConsumedUnits = consumedUnits;
        BillingMonth = billingMonth;
        TotalGovtCharges = totalGovtCharges;
        TotalIescoCharges = totalIescoCharges;
        IssueDate = issueDate;
        DueDate = dueDate;
        AmountBeforeDueDate = amountBeforeDueDate;
        AmountAfterDueDate = amountAfterDueDate;
    }
}
