using AppBilling.MonthNames;
using System;
using System.ComponentModel.DataAnnotations;
namespace Billing.BillingCalculations;

public class UpdateBillingCalculationDto
{
    [Required]
    public Guid MeterInfoId { get; set; }
    [Required]
    public decimal CurrentReading { get; set; }
    public decimal? PreviousReading { get; set; }
    [Required]
    public decimal ConsumedUnits { get; set; }
    [Required]
    public DateTime MeterReadingDate { get; set; }
    [Required]
    public decimal UnitsAmount { get; set; }
    [Required]
    public MonthName BillingMonth { get; set; }
    public decimal? TotalGovtCharges { get; set; }
    public decimal? TotalIescoCharges { get; set; }
    public DateTime? IssueDate { get; set; }
    public DateTime? DueDate { get; set; }
    public decimal? AmountBeforeDueDate { get; set; }
    public decimal? AmountAfterDueDate { get; set; }
}
