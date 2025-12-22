using Billing.MaintenanceBills;
using System;
using Volo.Abp.Application.Dtos;

namespace Billing.ElectricityBills;

public class ElectricityBillDto : FullAuditedEntityDto<Guid>
{
    public Guid MeterInfoId { get; set; }

    public string MeterNo { get; set; } = default!;

    public DateTime BillingMonth { get; set; }

    public decimal PreviousReading { get; set; }
    public decimal PresentReading { get; set; }
    public decimal ConsumedUnits { get; set; }

    public DateTime MeterReadingDate { get; set; }
    public DateTime IssueDate { get; set; }
    public DateTime DueDate { get; set; }

    public decimal CurrentMonthBill { get; set; }
    public decimal BillAdjustment { get; set; }
    public decimal AnyOtherCharges { get; set; }

    public decimal PayableDueDateAmount { get; set; }
    public decimal LPSurcharge { get; set; }
    public decimal PayableAfterDueDateAmount { get; set; }
    public BillStatus  Status { get; set; }
    public decimal Arrears { get; set; }
    public decimal TotalGovernmentCharges { get; set; }
    public decimal TotalIESCOCharges { get; set; }
    public decimal TotalSocietyCharges { get; set; } 

}