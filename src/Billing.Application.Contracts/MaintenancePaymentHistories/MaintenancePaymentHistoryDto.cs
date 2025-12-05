using System;
using Volo.Abp.Application.Dtos;

namespace Billing.MaintenancePaymentHistories;

public class MaintenancePaymentHistoryDto : AuditedEntityDto<Guid>
{
    public Guid MaintenanceBillId { get; set; }
    public string TransactionId { get; set; }
    public decimal PaymentReceived { get; set; }
    public DateTime PaymentDate { get; set; }
    public PaymentMethod Method { get; set; }
    public Guid? TenantId { get; set; }
}
