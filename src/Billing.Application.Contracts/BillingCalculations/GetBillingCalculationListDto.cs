using AppBilling.MonthNames;
using System;
using Volo.Abp.Application.Dtos;

namespace Billing.BillingCalculations;

public class GetBillingCalculationListDto : PagedAndSortedResultRequestDto
{
    public string? Filter { get; set; }
    public Guid MeterInfoId { get; set; }
    public decimal CurrentReading { get; set; }
    public decimal? PreviousReading { get; set; }
    public decimal ConsumedUnits { get; set; }
    public DateTime MeterReadingDate { get; set; }
    public decimal UnitsAmount { get; set; }
    public MonthName BillingMonth { get; set; }
    public decimal? TotalGovtCharges { get; set; }
    public decimal? TotalIescoCharges { get; set; }
    public DateTime? IssueDate { get; set; }
    public DateTime? DueDate { get; set; }
    public decimal? AmountBeforeDueDate { get; set; }
    public decimal? AmountAfterDueDate { get; set; }
}
