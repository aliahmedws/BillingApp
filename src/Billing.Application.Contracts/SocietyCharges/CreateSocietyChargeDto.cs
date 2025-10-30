namespace Billing.SocietyCharges;

public class CreateSocietyChargeDto
{
    public decimal? SecurityCharges { get; set; } = 0;
    public decimal? MaintenanceCharges { get; set; } = 0;
    public decimal? WaterCharges { get; set; } = 0;
    public decimal? OtherCharges { get; set; } = 0;
    public decimal? TotalSocietyCharges { get; set; } = 0;
}
