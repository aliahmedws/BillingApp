using Billing.MaintenancePaymentHistories;
using System;
using Volo.Abp.Application.Dtos;

namespace Billing.ElectricityPaymentHistories;

public class ElectricityPaymentHistoryDto : EntityDto<Guid>
{
    public string TransactionId { get; set; } = default!;
    public decimal PaymentReceived { get; set; }
    public DateTime PaymentDate { get; set; }
    public PaymentMethod Method { get; set; }
    public Guid ElectricityBillId { get; set; }
    public Guid? TenantId { get; set; }
}

