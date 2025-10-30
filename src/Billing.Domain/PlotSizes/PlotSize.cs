using System;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;

namespace Billing.PlotSizes;

public class PlotSize : FullAuditedAggregateRoot<Guid>
{
    public string SizeName { get; set; }           // e.g., "5 Marla", "1 Kanal"
    public decimal Area { get; set; }              // e.g., 1125.00
    public PlotUnit Unit { get; set; }             // enum instead of string
    public decimal? Length { get; set; }           // e.g., 30.00
    public decimal? Width { get; set; }            // e.g., 37.50
    public string? Description { get; set; }       // optional
    public bool IsActive { get; set; } = true;

    private PlotSize() { }

    internal PlotSize(
        Guid id,
        string sizeName,
        decimal area,
        PlotUnit unit,
        decimal? length = null,
        decimal? width = null,
        string? description = null,
        bool isActive = true)
        : base(id)
    {
        SetSizeName(sizeName);
        SetArea(area);
        Unit = unit;
        ChangeLength(length);
        ChangeWidth(width);
        ChangeDescription(description);
        IsActive = isActive;
    }

    internal PlotSize ChangeSizeName(string sizeName)
    {
        SetSizeName(sizeName);
        return this;
    }

    internal PlotSize ChangeArea(decimal area)
    {
        SetArea(area);
        return this;
    }

    internal PlotSize ChangeUnit(PlotUnit unit)
    {
        Unit = unit;
        return this;
    }

    internal PlotSize ChangeLength(decimal? length)
    {
        Length = length;
        return this;
    }

    internal PlotSize ChangeWidth(decimal? width)
    {
        Width = width;
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

    private void SetArea(decimal area)
    {
        if (area <= 0)
        {
            throw new BusinessException("PlotSize:AreaMustBePositive")
                .WithData("Area", area);
        }
        Area = area;
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
