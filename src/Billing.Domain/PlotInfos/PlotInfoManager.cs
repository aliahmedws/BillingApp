using Billing.PlotTypes;
using System;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Domain.Services;

namespace Billing.PlotInfos;

public class PlotInfoManager : DomainService
{
    private readonly IPlotInfoRepository _plotInfoRepository;

    public PlotInfoManager(IPlotInfoRepository plotInfoRepository)
    {
        _plotInfoRepository = plotInfoRepository;
    }

    public async Task<PlotInfo> CreateAsync(
        string plotNo,
        PlotType plotType,
        string streetNo,
        Guid plotSizeId,
        PlotStatus status,
        Guid blockId,
        Guid? consumerId,
        Guid phaseId,
        string? remarks = null)
    {
        Check.NotNullOrWhiteSpace(plotNo, nameof(plotNo));
        Check.NotNull(blockId, nameof(blockId));
        Check.NotNull(phaseId, nameof(phaseId));

        // 1️⃣ Check duplicate plot number in the same block
        var existingPlot = await _plotInfoRepository.FindByPlotNoAsync(plotNo, blockId);
        if (existingPlot != null)
        {
            throw new PlotAlreadyExistException(plotNo);
        }

        // 2️⃣ Check if the same consumer already owns a plot in the same block & phase
        if (consumerId.HasValue)
        {
            var existingConsumerPlot = await _plotInfoRepository.FindByConsumerAsync(consumerId.Value, blockId, phaseId);
            if (existingConsumerPlot != null)
            {
                throw new ConsumerAlreadyHasPlotException(consumerId.Value);
            }
        }

        return new PlotInfo(
            GuidGenerator.Create(),
            plotNo,
            plotType,
            streetNo,
            plotSizeId,
            status,
            blockId,
            consumerId,
            phaseId,
            remarks
        );
    }

    public async Task UpdateAsync(
        PlotInfo plot,
        string plotNo,
        PlotType plotType,
        string streetNo,
        Guid plotSizeId,
        PlotStatus status,
        Guid blockId,
        Guid? consumerId,
        Guid phaseId,
        string? remarks)
    {
        Check.NotNull(plot, nameof(plot));
        Check.NotNullOrWhiteSpace(plotNo, nameof(plotNo));
        Check.NotNull(blockId, nameof(blockId));
        Check.NotNull(phaseId, nameof(phaseId));

        // 1️⃣ Check duplicate PlotNo (ignore current one)
        var existingPlot = await _plotInfoRepository.FindByPlotNoAsync(plotNo, blockId);
        if (existingPlot != null && existingPlot.Id != plot.Id)
        {
            throw new PlotAlreadyExistException(plotNo);
        }

        // 2️⃣ Check duplicate assignment (same consumer, same phase, same block)
        if (consumerId.HasValue)
        {
            var existingConsumerPlot = await _plotInfoRepository.FindByConsumerAsync(consumerId.Value, blockId, phaseId);
            if (existingConsumerPlot != null && existingConsumerPlot.Id != plot.Id)
            {
                throw new ConsumerAlreadyHasPlotException(consumerId.Value);
            }
        }

        plot
            .ChangePlotNo(plotNo)
            .ChangePlotType(plotType)
            .ChangeStreetNo(streetNo)
            .ChangePlotSize(plotSizeId)
            .ChangeStatus(status)
            .ChangeBlock(blockId)
            .ChangeBuyer(consumerId)
            .ChangePhase(phaseId)
            .ChangeRemarks(remarks);
    }
}