using System.Collections.Generic;

namespace Billing.MaintenancePaymentHistories;

public class ImportResultDto
{

    public int SuccessCount { get; set; }
    public List<string> Errors { get; set; } = new();
}
