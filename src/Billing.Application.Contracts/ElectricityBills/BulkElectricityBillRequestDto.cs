using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Billing.ElectricityBills;

public class BulkElectricityBillRequestDto
{
    public DateTime BillingMonth { get; set; }
    public DateTime MeterReadingDate { get; set; }
    public DateTime IssueDate { get; set; }
    public DateTime DueDate { get; set; }
    public decimal AnyOtherCharges { get; set; }
    public decimal LpSurcharge { get; set; }

    public List<BulkElectricityBillRowDto> Items { get; set; } = new();

}
