using System;

namespace Billing.ElectricityPaymentHistories;

public class ElectricityPaymentExcelDto
{
    public Guid ElectricityBillId { get; set; }
    public string TransactionId { get; set; } = default!;
    public decimal PaymentReceived { get; set; }
    public DateTime PaymentDate { get; set; }
    public string Method { get; set; } = default!;
}
