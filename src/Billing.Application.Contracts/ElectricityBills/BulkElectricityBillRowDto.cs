using System;

namespace Billing.ElectricityBills;

public class BulkElectricityBillRowDto
{
    public Guid MeterInfoId { get; set; }
    public decimal PreviousReading { get; set; }
    public decimal PresentReading { get; set; }
    public decimal Arrears { get; set; }

}
