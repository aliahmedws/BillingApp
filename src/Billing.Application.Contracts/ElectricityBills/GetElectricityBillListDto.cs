using Billing.MaintenanceBills;
using System;
using Volo.Abp.Application.Dtos;

namespace Billing.ElectricityBills;

public class GetElectricityBillListDto : PagedAndSortedResultRequestDto
{
    public string? Filter { get; set; }

    public Guid? MeterInfoId { get; set; }

    public decimal? PreviousReading { get; set; }
    public decimal? PresentReading { get; set; }

    public DateTime? MeterReadingDate { get; set; }
    public DateTime? BillingMonth { get; set; }
    public DateTime? IssueDate { get; set; }
    public DateTime? DueDate { get; set; }

    public decimal? CurrentMonthBill { get; set; }
    public decimal? BillAdjustment { get; set; }
    public decimal? AnyOtherCharges { get; set; }
    public decimal? LPSurcharge { get; set; }
    public BillStatus? Status { get; set; }
}
