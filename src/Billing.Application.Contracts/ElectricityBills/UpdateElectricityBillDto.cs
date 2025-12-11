using Billing.MaintenanceBills;
using System;
using System.ComponentModel.DataAnnotations;

namespace Billing.ElectricityBills;

public class UpdateElectricityBillDto
{

    [Required]
    public Guid MeterInfoId { get; set; }

    [Required]
    public decimal PreviousReading { get; set; } = 0m;

    [Required]
    public decimal PresentReading { get; set; } = 0m;

    [Required]
    public DateTime MeterReadingDate { get; set; }

    [Required]
    public DateTime BillingMonth { get; set; }

    [Required]
    public DateTime IssueDate { get; set; }

    [Required]
    public DateTime DueDate { get; set; }

    [Required]
    public decimal CurrentMonthBill { get; set; }

    [Required]
    public decimal BillAdjustment { get; set; } = 0m;

    [Required]
    public decimal AnyOtherCharges { get; set; } = 0m;

    [Required]
    public decimal LPSurcharge { get; set; } = 0m;

    [Required]
    public BillStatus Status { get; set; }

    [Required]
    public decimal Arrears { get; set; } = 0m;
}
