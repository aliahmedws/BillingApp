using System;
using System.ComponentModel.DataAnnotations;

namespace Billing.MaintenancePaymentHistories;

public class CreateMaintenancePaymentHistoryDto
{
    [Required]
    public Guid MaintenanceBillId { get; set; }

    [Required]
    [StringLength(MaintenancePaymentHistoryConsts.MaxTransactionIdLength)]
    public string TransactionId { get; set; }

    [Required]
    public decimal PaymentReceived { get; set; }

    [Required]
    public DateTime PaymentDate { get; set; }

    [Required]
    public PaymentMethod Method { get; set; }
    public Guid? TenantId { get; set; }
}
