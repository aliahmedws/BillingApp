using System;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Domain.Services;

namespace Billing.MaintenancePaymentHistories; 

public class MaintenancePaymentHistoryManager : DomainService
{
    private readonly IMaintenancePaymentHistoryRepository _maintenancePaymentHistoryRepository;
    public MaintenancePaymentHistoryManager(IMaintenancePaymentHistoryRepository maintenancePaymentHistoryRepository)
    {
        _maintenancePaymentHistoryRepository = maintenancePaymentHistoryRepository;
    }

    // Create Async
    public async Task<MaintenancePaymentHistory> CreateAsync(
                Guid maintenanceBillId,
                string transactionId,
                decimal paymentReceived,
                DateTime paymentDate,
                PaymentMethod method
        )
    {
        Check.NotNullOrWhiteSpace(transactionId, nameof(transactionId));

        var existingTransaction = await _maintenancePaymentHistoryRepository.FindByTransactionIdAsync(transactionId);
        if (existingTransaction != null)
        {
            throw new MaintenancePaymentHistoryAlreadyExistsException(transactionId);
        }

        if (paymentReceived < 0)
        {
            throw new PaymentReceivedNegativeException(paymentReceived);
        }

        return new MaintenancePaymentHistory(
            GuidGenerator.Create(),
            maintenanceBillId,
            transactionId,
            paymentReceived,
            paymentDate,
            method
            );


    }
    //Update Async
    public async Task UpdateAsync(
        MaintenancePaymentHistory maintenancePaymentHistory,
        string transactionId,
        decimal paymentReceived,
        DateTime paymentDate,
        PaymentMethod method
        )
    {
        Check.NotNull(maintenancePaymentHistory, nameof(MaintenancePaymentHistory));

        Check.NotNullOrWhiteSpace(transactionId, nameof(transactionId));

        var existingTransaction = await _maintenancePaymentHistoryRepository.FindByTransactionIdAsync(transactionId);
        if (existingTransaction != null && existingTransaction.Id != maintenancePaymentHistory.Id)
        {
            throw new MaintenancePaymentHistoryAlreadyExistsException(transactionId);
        }

        if (paymentReceived < 0)
        {
            throw new PaymentReceivedNegativeException(paymentReceived);
        }

        maintenancePaymentHistory.UpdateTransaction(transactionId);
        maintenancePaymentHistory.PaymentReceived = paymentReceived;
        maintenancePaymentHistory.PaymentDate = paymentDate;
        maintenancePaymentHistory.Method = method;

    }

}



