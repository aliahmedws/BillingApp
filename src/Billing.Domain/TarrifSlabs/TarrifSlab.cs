using System;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.Identity;
using Volo.Abp.MultiTenancy;

namespace Billing.TarrifSlabs;

public class TarrifSlab : FullAuditedAggregateRoot<Guid>, IMultiTenant
{
    public decimal LowerSlab { get; set; }
    public decimal? UpperSlab { get; set; }
    public decimal UnitPrice { get; set; }
    public Guid? TenantId { get; set; }
    public virtual IdentityUser Creator { get; set; }
    public virtual IdentityUser LastModifier { get; set; }
    public TarrifSlab(){ }
    internal TarrifSlab(Guid id, decimal lowerSlab, decimal? upperSlab, decimal unitPrice) : base(id)
    {
        LowerSlab = lowerSlab;
        UpperSlab = upperSlab;
        UnitPrice = unitPrice;
    }
}
