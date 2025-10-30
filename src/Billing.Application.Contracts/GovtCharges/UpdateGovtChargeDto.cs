namespace Billing.GovtCharges;

public class UpdateGovtChargeDto
{
    public decimal? Ed { get; set; }
    public decimal? TvFee { get; set; }
    public decimal? GST { get; set; }
    public decimal? IncomeTax { get; set; }
    public decimal? ExtraTax { get; set; }
    public decimal? FurtherTax { get; set; }
    public decimal? NjSurcharge { get; set; }
    public decimal? SalesTax { get; set; }
    public decimal? FcSurcharge { get; set; }
    public decimal? TrSurcharge { get; set; }
    public decimal? TaxOnFpa { get; set; }
    public decimal? TotalTaxes { get; set; }
}
