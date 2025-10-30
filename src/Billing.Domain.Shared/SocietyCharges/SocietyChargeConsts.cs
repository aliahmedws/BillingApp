namespace Billing.SocietyCharges;

public class SocietyChargeConsts
{
    // Decimal limits
    public const decimal MinValue = 0m;
    public const decimal MaxValue = 9999999.999m;
    // Decimal precision (for EF config)
    public const int DecimalPrecision = 10;
    public const int DecimalScale = 4;
    // Common validation message
    public const string DecimalValidationMessage = "Value must be positive and up to 4 decimal places.";
}
