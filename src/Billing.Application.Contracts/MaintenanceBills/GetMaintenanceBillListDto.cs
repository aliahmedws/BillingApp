using System;
using Volo.Abp.Application.Dtos;

namespace Billing.MaintenanceBills;

public class GetMaintenanceBillListDto : PagedAndSortedResultRequestDto
{
    public string? Filter { get; set; }
    public Guid? ConsumerId { get; set; }
    public Guid? PlotInfoId { get; set; }
    public DateTime? BillingMonth { get; set; }
    public BillStatus? Status { get; set; }
}
