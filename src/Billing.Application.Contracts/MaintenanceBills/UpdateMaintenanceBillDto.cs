using System;

namespace Billing.MaintenanceBills;

public class UpdateMaintenanceBillDto
{
    public Guid ConsumerId { get; set; }
    public Guid PlotInfoId { get; set; }
    public DateTime BillingMonth { get; set; }
    public DateTime IssueDate { get; set; }
    public DateTime DueDate { get; set; }
    public decimal WaterCharges { get; set; }
    public decimal SecurityCharges { get; set; }
    public decimal CurrentBill { get; set; }
    public decimal Arrears { get; set; }
    public decimal OtherCharges { get; set; }
    public decimal RefundOrBenefit { get; set; }
    public decimal AnyOtherWorkCharges { get; set; }
    public decimal PaymentBeforeDueDate { get; set; }
    public decimal LatePaymentSurcharge { get; set; }
    public decimal PayableAfterDueDate { get; set; }
    public BillStatus Status { get; set; }
    public int? PartialMonths { get; set; }
    public decimal? PartialMonthlyAmount { get; set; }
}
