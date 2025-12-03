using System;
using Volo.Abp.Application.Dtos;

namespace Billing.MaintenanceBills;

public class MaintenanceBillDto : AuditedEntityDto<Guid>
{
    public Guid? TenantId { get; set; }
    public Guid ConsumerId { get; set; }
    public string? ConsumerFullName { get; set; }
    public Guid PlotInfoId { get; set; }
    public string? PlotNo { get; set; }
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
}
