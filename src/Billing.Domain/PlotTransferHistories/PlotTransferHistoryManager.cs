using Billing.ConsumerPersonalInfos;
using Billing.PlotInfos;
using System;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Domain.Services;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Users;

namespace Billing.PlotTransferHistories;

public class PlotTransferHistoryManager : DomainService
{
    private readonly IPlotTransferHistoryRepository _plotTransferHistoryRepository;
    private readonly IConsumerPersonalInfoRepository _consumerPersonalInfoRepository;
    private readonly IPlotInfoRepository _plotInfoRepository;
    private readonly ICurrentTenant _currentTenant;
    private readonly ICurrentUser _currentUser;
    public PlotTransferHistoryManager(
        IPlotTransferHistoryRepository plotTransferHistoryRepository,
        IConsumerPersonalInfoRepository consumerPersonalInfoRepository,
        IPlotInfoRepository plotInfoRepository,
        ICurrentTenant currentTenant,
        ICurrentUser currentUser
        )
    {
        _plotTransferHistoryRepository = plotTransferHistoryRepository;
        _consumerPersonalInfoRepository = consumerPersonalInfoRepository;
        _plotInfoRepository = plotInfoRepository;
        _currentTenant = currentTenant;
        _currentUser = currentUser;
    }

    public async Task<PlotTransferHistory> CreateAsync(
        Guid plotId,
        Guid fromConsumerId,
        Guid toConsumerId,
        DateTime transferDate,
        TransferType transferType,
        string registryNo,
        decimal considerationAmount,
        string? remarks = null)
    {
        Check.NotNull(plotId, nameof(plotId));
        Check.NotNull(fromConsumerId, nameof(fromConsumerId));
        Check.NotNull(toConsumerId, nameof(toConsumerId));
        Check.NotNull(transferDate, nameof(transferDate));
        Check.NotNullOrWhiteSpace(registryNo, nameof(registryNo));

        if (fromConsumerId == toConsumerId)
        {
            throw new InvalidConsumerTransferException(toConsumerId);
        }

        var existingTransfer = await _plotTransferHistoryRepository.FindByRegistryNoAsync(registryNo);
        if (existingTransfer != null)
        {
            throw new PlotTransferRegistryAlreadyExistsException(registryNo);
        }

         await _plotInfoRepository.ChangePlotOwnerAsync(fromConsumerId, toConsumerId, plotId);

        return new PlotTransferHistory(
            GuidGenerator.Create(),
            plotId,
            fromConsumerId,
            toConsumerId,
            transferDate,
            transferType,
            registryNo,
            considerationAmount,
            null,
            null,
            false,
            remarks,
            _currentTenant.Id
        );
    }

    public async Task UpdateAsync(
        PlotTransferHistory history,
        Guid plotId,
        Guid fromConsumerId,
        Guid toConsumerId,
        DateTime transferDate,
        TransferType transferType,
        string registryNo,
        decimal considerationAmount,
        string? remarks)
    {
        Check.NotNull(history, nameof(history));
        Check.NotNull(fromConsumerId, nameof(fromConsumerId));
        Check.NotNull(toConsumerId, nameof(toConsumerId));
        Check.NotNull(plotId, nameof(plotId));
        Check.NotNull(transferDate, nameof(transferDate));
        Check.NotNullOrWhiteSpace(registryNo, nameof(registryNo));

        if (fromConsumerId == toConsumerId)
        {
            throw new InvalidConsumerTransferException(toConsumerId);
        }

        var existingTransfer = await _plotTransferHistoryRepository.FindByRegistryNoAsync(registryNo);
        if (existingTransfer != null && existingTransfer.Id != history.Id)
        {
            throw new PlotTransferRegistryAlreadyExistsException(registryNo);
        }

        history
            .SetFromConsumer(fromConsumerId)
            .SetToConsumer(toConsumerId)
            .ChangeConsiderationAmount(considerationAmount)
            .SetTenant(_currentTenant.Id);
    }

    public async Task ApproveAsync(Guid transferId)
    {
        var history = await _plotTransferHistoryRepository.GetAsync(transferId);
        if (history.IsApproved)
        {
            throw new PlotTransferAlreadyApprovedException(history.RegistryNo);
        }

        history.Approve(_currentUser.Id);
        history.SetTenant(_currentTenant.Id);
        await _plotTransferHistoryRepository.UpdateAsync(history);
    }

    public async Task RejectAsync(Guid transferId, string? reason = null)
    {
        var history = await _plotTransferHistoryRepository.GetAsync(transferId);
        if (!history.IsApproved && string.IsNullOrWhiteSpace(reason))
        {
            throw new PlotTransferAlreadyRejectedException(history.RegistryNo);
        }

        history.Reject(reason, _currentUser.Id);
        history.SetTenant(_currentTenant.Id);
        await _plotTransferHistoryRepository.UpdateAsync(history);
    }
}
