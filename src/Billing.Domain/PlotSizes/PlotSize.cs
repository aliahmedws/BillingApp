using Billing.PlotInfos;
using Billing.SocietyCharges;
using System;
using System.Collections.Generic;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.Identity;
using Volo.Abp.MultiTenancy;

namespace Billing.PlotSizes;

public class PlotSize : FullAuditedAggregateRoot<Guid>, IMultiTenant
{
    public string SizeName { get; set; }           // e.g., "5 Marla", "1 Kanal"
    public decimal Area { get; set; }              // e.g., 1125.00
    public PlotUnit Unit { get; set; }             // enum instead of string
    public decimal? Length { get; set; }           // e.g., 30.00
    public decimal? Width { get; set; }            // e.g., 37.50
    public string? Description { get; set; }       // optional
    public bool IsActive { get; set; } = true;
    public Guid? TenantId { get; set; }
    public ICollection<SocietyCharge> SocietyCharges { get; set; }
    public virtual ICollection<PlotInfo> PlotInfos { get; set; }
    public virtual IdentityUser Creator { get; set; }
    public virtual IdentityUser LastModifier { get; set; }

    private PlotSize() 
    {
        PlotInfos = new List<PlotInfo>();
 
    }

    internal PlotSize(
        Guid id,
        string sizeName,
        decimal area,
        PlotUnit unit,
        decimal? length = null,
        decimal? width = null,
        string? description = null,
        bool isActive = true,
        Guid? tenantId = null)
        : base(id)
    {
        SetSizeName(sizeName);
        SetValue(area, nameof(area));
        Unit = unit;
        SetValue(length ?? 0m, nameof(length));
        SetValue(width  ?? 0m, nameof(width));
        SetDescription(description);
        IsActive = isActive;
        TenantId = tenantId;
    }

    internal PlotSize ChangeSizeName(string sizeName)
    {
        SetSizeName(sizeName);
        return this;
    }

    internal PlotSize SetTenant(Guid? tenantId)
    {
        TenantId = tenantId;
        return this;
    }

    internal PlotSize ChangeArea(decimal area)
    {
        SetValue(area, nameof(area));
        return this;
    }
    internal PlotSize ChangeLength(decimal? length)
    {
        SetValue(length, nameof(length));
        return this;
    }
    internal PlotSize ChangeWidth(decimal? width)
    {
        SetValue(width, nameof(width));
        return this;
    }

    internal PlotSize ChangeUnit(PlotUnit unit)
    {
        Unit = unit;
        return this;
    }

    internal PlotSize ChangeDescription(string? description)
    {
        SetDescription(description);
        return this;
    }

    internal PlotSize SetActiveStatus(bool isActive)
    {
        IsActive = isActive;
        return this;
    }

    private void SetSizeName(string sizeName)
    {
        SizeName = Check.NotNullOrWhiteSpace(sizeName, nameof(sizeName), maxLength: PlotSizeConsts.MaxSizeNameLength);
    }

    private void SetValue(decimal? value, string valueName)
    {
        if (value.HasValue && value.Value < 0)
        {
            throw new NegativeValueException(value.Value, valueName);
        }

        if (valueName.ToLower() == "area")
        {
            Area = value ?? 0m;
        }

        if (valueName.ToLower() == "length")
        {
            Length = value;
        }

        if (valueName.ToLower() == "width")
        {
            Width = value;
        }
    }


    private void SetDescription(string? description)
    {
        if (!description.IsNullOrWhiteSpace())
        {
            Description = Check.Length(description, nameof(description), PlotSizeConsts.MaxDescriptionLength, 0);
        }
        else
        {
            Description = null;
        }
    }
}
