using Billing.ElectricityBills;
using Billing.MaintenancePaymentHistories;
using System;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace Billing.ElectricityPaymentHistories;

public class ElectricityPaymentHistory : FullAuditedAggregateRoot<Guid>, IMultiTenant
{
    public string TransactionId { get; set; }
    public decimal PaymentReceived { get; set; } = 0m;
    public DateTime PaymentDate { get; set; }
    public PaymentMethod Method { get; set; }
    public Guid? TenantId { get; set; }
    public Guid ElectricityBillId { get; set; }
    public virtual ElectricityBill ElectricityBills { get; set; }

    private ElectricityPaymentHistory()
    {
    }

    internal ElectricityPaymentHistory(
             Guid id,
             Guid electricityBillId,
             string transactionId,
             decimal paymentReceived,
             DateTime paymentDate,
             PaymentMethod method
    ) : base(id)
    {
        SetTransactionId(transactionId);
        SetPaymentReceived(paymentReceived);
        PaymentDate = paymentDate;
        Method = method;
        ElectricityBillId = electricityBillId;
    }

    internal ElectricityPaymentHistory ChangeTransactionId(string transactionId)
    {
        SetTransactionId(transactionId);
        return this;
    }
    internal ElectricityPaymentHistory ChangePaymentReceived(decimal paymentReceived)
    {
        SetPaymentReceived(paymentReceived);
        return this;
    }

    private void SetTransactionId(string transactionId)
    {
        TransactionId = Check.NotNullOrWhiteSpace(
                        transactionId,
                        nameof(transactionId),
                        maxLength: ElectricityPaymentHistoryConsts.MaxTransactionIdLength);
    }
    private void SetPaymentReceived(decimal paymentReceived)
    {
        if (paymentReceived < 0 || paymentReceived > ElectricityPaymentHistoryConsts.MaxPaymentReceived)
        {
            throw new BillAmountRangeException(paymentReceived);
        }

        PaymentReceived = paymentReceived;
    }

}


