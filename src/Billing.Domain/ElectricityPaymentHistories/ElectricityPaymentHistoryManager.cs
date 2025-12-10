using Billing.MaintenancePaymentHistories;
using System;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Domain.Services;

namespace Billing.ElectricityPaymentHistories;
public class ElectricityPaymentHistoryManager : DomainService
{
    private readonly IElectricityPaymentHistoryRepository _electricityPaymentHistoryRepository;
    public ElectricityPaymentHistoryManager(IElectricityPaymentHistoryRepository electricityPaymentHistoryRepository)
    {
        _electricityPaymentHistoryRepository = electricityPaymentHistoryRepository;
    }

    public async Task<ElectricityPaymentHistory> CreateAsync(
                Guid electricityBillId,
                string transactionId,
                decimal paymentReceived,
                DateTime paymentDate,
                PaymentMethod method
       )
    {
        Check.NotNullOrWhiteSpace(transactionId, nameof(transactionId));

        if (paymentReceived < 0)
        {
            throw new BillAmountRangeException(paymentReceived);
        }

        var existingPayment = await _electricityPaymentHistoryRepository.FindByTransactionIdAsync(transactionId);

        if (existingPayment != null)
        {
            throw new ElectricityPaymentAlreadyExistsException(transactionId);
        }

        return new ElectricityPaymentHistory(
            GuidGenerator.Create(),
            electricityBillId,
            transactionId,
            paymentReceived,
            paymentDate,
            method
            );
    }


    public async Task UpdateElectricityPaymentHistoryAsync(
        ElectricityPaymentHistory paymentHistory,
        string transactionId,
        decimal paymentReceived,
        DateTime paymentDate,
        PaymentMethod method
    )
    {
        Check.NotNull(paymentHistory, nameof(paymentHistory));

        if (paymentReceived < 0)
        {
            throw new BillAmountRangeException(paymentReceived);
        }

        paymentHistory.ChangeTransactionId(transactionId);
        paymentHistory.ChangePaymentReceived(paymentReceived);
        paymentHistory.PaymentDate = paymentDate;
        paymentHistory.Method = method;
    }
}

