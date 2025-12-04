using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Domain.Services;
using Volo.Abp.MultiTenancy;
using Volo.Abp.TenantManagement;

namespace Billing.PlotSizes;

public class PlotSizeManager : DomainService
{
    private readonly IPlotSizeRepository _plotSizeRepository;


    public PlotSizeManager(IPlotSizeRepository plotSizeRepository)
    {
        _plotSizeRepository = plotSizeRepository;
      
    }

    public async Task<PlotSize> CreateAsync(
        string sizeName,
        decimal area,
        PlotUnit unit,
        decimal? length,
        decimal? width,
        string? description = null,
        bool isActive = true)
    {
        Check.NotNullOrWhiteSpace(sizeName, nameof(sizeName));
        Check.NotNull(area, nameof(area));
        Check.NotNull(unit, nameof(unit));

        return new PlotSize(
            GuidGenerator.Create(),
            sizeName,
            area,
            unit,
            length,
            width,
            description,
            isActive,
            CurrentTenant.Id
        );
    }

    public async Task<PlotSize> UpdateAsync(
        PlotSize plotSize,
        string newName,
        decimal area,
        PlotUnit unit,
        decimal? length,
        decimal? width,
        string? description,
        bool isActive)
    {
        Check.NotNull(plotSize, nameof(plotSize));
        Check.NotNullOrWhiteSpace(newName, nameof(newName));
        Check.NotNull(area, nameof(area));
        Check.NotNull(unit, nameof(unit));

      return plotSize
             .ChangeSizeName(newName)
             .ChangeArea(area)
             .ChangeUnit(unit)
             .ChangeLength(length)
             .ChangeWidth(width)
             .ChangeDescription(description)
             .SetActiveStatus(isActive)
             .SetTenant(CurrentTenant.Id);

    }
}
