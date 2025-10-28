using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Domain.Services;

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
        decimal? length = null,
        decimal? width = null,
        string? description = null,
        bool isActive = true)
    {
        Check.NotNullOrWhiteSpace(sizeName, nameof(sizeName));
        Check.NotNull(unit, nameof(unit));

        //var existingPlotSize = await _plotSizeRepository.FindByNameAsync(sizeName);
        //if (existingPlotSize != null)
        //{
        //    throw new PlotSizeAlreadyExistsException(sizeName);
        //}

        return new PlotSize(
            GuidGenerator.Create(),
            sizeName,
            area,
            unit,
            length,
            width,
            description,
            isActive
        );
    }

    public async Task UpdateAsync(
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
        Check.NotNull(unit, nameof(unit));

        //var existingPlotSize = await _plotSizeRepository.FindByNameAsync(newName);
        //if (existingPlotSize != null && existingPlotSize.Id != plotSize.Id)
        //{
        //    throw new PlotSizeAlreadyExistsException(newName);
        //}

        plotSize
            .ChangeSizeName(newName)
            .ChangeArea(area)
            .ChangeUnit(unit)
            .ChangeLength(length)
            .ChangeWidth(width)
            .ChangeDescription(description)
            .SetActiveStatus(isActive);
    }
}
