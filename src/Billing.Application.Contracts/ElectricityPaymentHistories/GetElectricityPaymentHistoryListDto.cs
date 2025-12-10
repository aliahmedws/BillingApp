using Billing.MaintenancePaymentHistories;
using System;
using Volo.Abp.Application.Dtos;

namespace Billing.ElectricityPaymentHistories;

public class GetElectricityPaymentHistoryListDto : PagedAndSortedResultRequestDto
{
    public string? Filter { get; set; }
    public string? TransactionId { get; set; }
    public decimal? MaxPaymentReceived { get; set; }
    public PaymentMethod? Method { get; set; }
    public Guid? ElectricityBillId { get; set; }

}

