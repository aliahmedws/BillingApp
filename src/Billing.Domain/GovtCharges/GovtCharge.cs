using System;
using Volo.Abp.Domain.Entities.Auditing;

namespace Billing.GovtCharges;

public class GovtCharge : FullAuditedAggregateRoot<Guid>
{
    public decimal? Ed { get; set; }
    public decimal? TvFee { get; set; }
    public decimal? GST { get; set; }
    public decimal? IncomeTax { get; set; }
    public decimal?  ExtraTax { get; set; }
    public decimal? FurtherTax { get; set; }
    public decimal? NjSurcharge { get; set; }
    public decimal? SalesTax { get; set; }
    public decimal? FcSurcharge { get; set; }
    public decimal? TrSurcharge { get; set; }
    public decimal? TaxOnFpa { get; set; }
    public decimal? TotalTaxes { get; set; }

    private GovtCharge()
    {
    }

    internal GovtCharge(
        Guid id,
        decimal? ed,
        decimal? tvFee,
        decimal? gst,
        decimal? incomeTax,
        decimal? extraTax,
        decimal? furtherTax,
        decimal? njSurcharge,
        decimal? salesTax,
        decimal? fcSurcharge,
        decimal? trSurcharge,
        decimal? taxOnFpa,
        decimal? totalTaxes
        )
        : base(id)
    {
        Ed = ed;
        TvFee = tvFee;
        GST = gst;
        IncomeTax = incomeTax;
        ExtraTax = extraTax;
        FurtherTax = furtherTax;
        NjSurcharge = njSurcharge;
        SalesTax = salesTax;
        FcSurcharge = fcSurcharge;
        TrSurcharge = trSurcharge;
        TaxOnFpa = taxOnFpa;
        TotalTaxes = totalTaxes;
    }
}
