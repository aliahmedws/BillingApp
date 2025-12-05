using Billing.ConsumerPersonalInfos;
using Billing.MaintenancePaymentHistories;
using Billing.PlotInfos;
using Billing.SocietyCharges;
using System;
using System.Collections.Generic;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace Billing.MaintenanceBills;

public class MaintenanceBill : FullAuditedAggregateRoot<Guid>, IMultiTenant
{
    public Guid? TenantId { get; set; }

    public Guid ConsumerId { get; private set; }
    public virtual ConsumerPersonalInfo ConsumerPersonalInfos { get; set; }
    public ICollection<MaintenancePaymentHistory> MaintenancePaymentHistories { get; set; }

    public Guid PlotInfoId { get; private set; }
    public virtual PlotInfo PlotInfos { get; set; }

    public DateTime BillingMonth { get; private set; }       // e.g. 2025-09-01
    public DateTime IssueDate { get; private set; }
    public DateTime DueDate { get; private set; }

    //public decimal MainMiscCharge { get; private set; }
    public decimal WaterCharges { get; private set; }
    public decimal SecurityCharges { get; private set; }
    public decimal CurrentBill { get; private set; }

    public decimal Arrears { get; private set; }
    public decimal OtherCharges { get; private set; }
    public decimal RefundOrBenefit { get; private set; }
    public decimal AnyOtherWorkCharges { get; private set; }

    public decimal PaymentBeforeDueDate { get; private set; }
    public decimal LatePaymentSurcharge { get; private set; }
    public decimal PayableAfterDueDate { get; private set; }

    public BillStatus Status { get; private set; } = BillStatus.Unpaid;

    private MaintenanceBill()
    {
    }

    internal MaintenanceBill(
        Guid id,
        Guid consumerId,
        Guid plotInfoId,
        DateTime billingMonth,
        DateTime issueDate,
        DateTime dueDate,
        decimal waterCharges,
        decimal securityCharges,
        decimal currentBill,
        decimal arrears,
        decimal otherCharges,
        decimal refundOrBenefit,
        decimal anyOtherWorkCharges,
        decimal paymentBeforeDueDate,
        decimal latePaymentSurcharge,
        decimal payableAfterDueDate)
        : base(id)
    {
        ConsumerId = Check.NotNull(consumerId, nameof(consumerId));
        PlotInfoId = Check.NotNull(plotInfoId, nameof(plotInfoId));

        BillingMonth = billingMonth;
        IssueDate = issueDate;
        DueDate = dueDate;

        WaterCharges = waterCharges;
        SecurityCharges = securityCharges;
        CurrentBill = currentBill;

        Arrears = arrears;
        OtherCharges = otherCharges;
        RefundOrBenefit = refundOrBenefit;
        AnyOtherWorkCharges = anyOtherWorkCharges;

        PaymentBeforeDueDate = paymentBeforeDueDate;
        LatePaymentSurcharge = latePaymentSurcharge;
        PayableAfterDueDate = payableAfterDueDate;
    }

    internal MaintenanceBill SetTenant(Guid? tenantId)
    {
        TenantId = tenantId;
        return this;
    }

    internal MaintenanceBill ChangeDates(DateTime billingMonth, DateTime issueDate, DateTime dueDate)
    {
        BillingMonth = billingMonth;
        IssueDate = issueDate;
        DueDate = dueDate;
        return this;
    }

    internal MaintenanceBill ChangeCharges(
        decimal waterCharges,
        decimal securityCharges,
        decimal currentBill,
        decimal arrears,
        decimal otherCharges,
        decimal refundOrBenefit,
        decimal anyOtherWorkCharges,
        decimal paymentBeforeDueDate,
        decimal latePaymentSurcharge,
        decimal payableAfterDueDate)
    {
        WaterCharges = waterCharges;
        SecurityCharges = securityCharges;
        CurrentBill = currentBill;

        Arrears = arrears;
        OtherCharges = otherCharges;
        RefundOrBenefit = refundOrBenefit;
        AnyOtherWorkCharges = anyOtherWorkCharges;

        PaymentBeforeDueDate = paymentBeforeDueDate;
        LatePaymentSurcharge = latePaymentSurcharge;
        PayableAfterDueDate = payableAfterDueDate;

        return this;
    }

    internal MaintenanceBill ChangeStatus(BillStatus status)
    {
        Status = status;
        return this;
    }

    internal MaintenanceBill MarkAsPaid()
    {
        Status = BillStatus.Paid;
        return this;
    }

    internal MaintenanceBill MarkAsCancelled()
    {
        Status = BillStatus.Cancelled;
        return this;
    }
}