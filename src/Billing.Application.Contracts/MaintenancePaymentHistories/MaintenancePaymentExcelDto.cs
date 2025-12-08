using Billing.MaintenanceBills;
using System;

namespace Billing.MaintenancePaymentHistories;

public class MaintenancePaymentExcelDto
{
    public Guid MaintenanceBillId { get; set; }
    public string TransactionId { get; set; } = default!;
    public decimal PaymentReceived { get; set; }
    public DateTime PaymentDate { get; set; }
    public string Method { get; set; } = default!;
}
