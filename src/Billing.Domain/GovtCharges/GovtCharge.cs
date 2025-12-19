using System;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.Identity;
using Volo.Abp.MultiTenancy;

namespace Billing.GovtCharges;

public class GovtCharge : FullAuditedAggregateRoot<Guid>, IMultiTenant
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
    public Guid? TenantId { get; set; }
    public virtual IdentityUser LastModifier { get; set; }
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
        Ed = ed ?? 0m;
        TvFee = tvFee ?? 0m;
        GST = gst ?? 0m;
        IncomeTax = incomeTax ?? 0m;
        ExtraTax = extraTax ?? 0m;
        FurtherTax = furtherTax ?? 0m;
        NjSurcharge = njSurcharge ?? 0m;
        SalesTax = salesTax ?? 0m;
        FcSurcharge = fcSurcharge ?? 0m;
        TrSurcharge = trSurcharge ?? 0m;
        TaxOnFpa = taxOnFpa ?? 0m;
        TotalTaxes = totalTaxes ?? 0m;
    }
}
