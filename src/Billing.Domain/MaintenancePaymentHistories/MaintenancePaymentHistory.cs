using Billing.MaintenanceBills;
using System;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace Billing.MaintenancePaymentHistories;

public class MaintenancePaymentHistory : FullAuditedAggregateRoot<Guid>, IMultiTenant
{
    public Guid? TenantId { get; set; }
    public Guid MaintenanceBillId { get; set; }
    public virtual MaintenanceBill MaintenanceBills { get; set; }
    public string TransactionId { get; set; }
    public decimal PaymentReceived { get; set; } = 0m;
    public DateTime PaymentDate { get; set; }
    public PaymentMethod Method { get; set; }

    private MaintenancePaymentHistory()
    {

    }

    internal MaintenancePaymentHistory(
        Guid id,
        Guid maintenanceBillId,
        string transactionId,
        decimal paymentReceived,
        DateTime paymentDate,
        PaymentMethod method
        ) : base(id)
    {
        MaintenanceBillId = Check.NotNull(maintenanceBillId, nameof(MaintenanceBillId));

        SetTransactionId(transactionId);
        PaymentReceived = paymentReceived;
        PaymentDate = paymentDate;
        Method = method;
    }

    internal MaintenancePaymentHistory UpdateTransaction(string transactionId)
    {
        SetTransactionId(transactionId);
        return this;
    }

    private void SetTransactionId(string transactionId)
    {
        TransactionId = Check.NotNullOrWhiteSpace(
            transactionId,
            nameof(transactionId),
            maxLength: MaintenancePaymentHistoryConsts.MaxTransactionIdLength
            );
    }
}
