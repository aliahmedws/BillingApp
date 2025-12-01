using System;

namespace Billing.MaintenanceBills;

public class GenerateMaintenanceBillsDto
{
    public DateTime BillingMonth { get; set; }
    public DateTime IssueDate { get; set; }
    public DateTime DueDate { get; set; }
    public decimal LatePaymentSurcharge { get; set; } = 0m;
}

public class GenerateMaintenanceBillsResultDto
{
    public int CreatedCount { get; set; }
    public int SkippedAlreadyExistCount { get; set; }
    public int SkippedNoSocietyChargesCount { get; set; }
}
