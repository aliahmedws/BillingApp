using Billing.MaintenancePaymentHistories;
using System;
using System.ComponentModel.DataAnnotations;

namespace Billing.ElectricityPaymentHistories;

public class UpdateElectricityPaymentHistoryDto
{
    [Required]
    [StringLength(ElectricityPaymentHistoryConsts.MaxTransactionIdLength)]
    public string TransactionId { get; set; } = default!;

    [Required]
    [Range(0, (double)ElectricityPaymentHistoryConsts.MaxPaymentReceived)]
    public decimal PaymentReceived { get; set; } = 0m;

    [Required]
    public DateTime PaymentDate { get; set; }

    [Required]
    public PaymentMethod Method { get; set; }

    [Required]
    public Guid ElectricityBillsId { get; set; }

}
