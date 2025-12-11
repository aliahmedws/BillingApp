using Billing.MaintenanceBills;
using System;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Domain.Services;

namespace Billing.MaintenancePaymentHistories; 

public class MaintenancePaymentHistoryManager : DomainService
{
    private readonly IMaintenancePaymentHistoryRepository _paymentRepository;
    private readonly IMaintenanceBillRepository _billRepository;

    public MaintenancePaymentHistoryManager(
        IMaintenancePaymentHistoryRepository paymentRepository,
        IMaintenanceBillRepository billRepository)
    {
        _paymentRepository = paymentRepository;
        _billRepository = billRepository;
    }

    public async Task<MaintenancePaymentHistory> CreateAsync(
        Guid maintenanceBillId,
        string transactionId,
        decimal paymentReceived,
        DateTime paymentDate,
        PaymentMethod method)
    {
        Check.NotNullOrWhiteSpace(transactionId, nameof(transactionId));
        Check.NotNull(maintenanceBillId, nameof(maintenanceBillId));
        Check.NotNull(paymentReceived, nameof(paymentReceived));
        Check.NotNull(paymentDate, nameof(paymentDate));
        Check.NotNull(method, nameof(method));

        if (paymentReceived < 0)
            throw new PaymentReceivedNegativeException(paymentReceived);

        var existingTransaction = await _paymentRepository.FindByTransactionIdAsync(transactionId);
        if (existingTransaction != null)
            throw new MaintenancePaymentHistoryAlreadyExistsException(transactionId);

        var bill = await _billRepository.GetAsync(maintenanceBillId);

        var arrears = bill.PaymentBeforeDueDate - paymentReceived;

        if (arrears == 0)
        {
            bill.Status = BillStatus.Paid;
            bill.Arrears = 0;
        }
        else
        {
            bill.Status = BillStatus.PartiallyPaid;
            bill.Arrears = arrears;
        }

        var history = new MaintenancePaymentHistory(
            GuidGenerator.Create(),
            maintenanceBillId,
            transactionId,
            paymentReceived,
            paymentDate,
            method
        );

        await _billRepository.UpdateAsync(bill);

        return history;
    }

    public async Task UpdateAsync(
        MaintenancePaymentHistory history,
        string transactionId,
        decimal paymentReceived,
        DateTime paymentDate,
        PaymentMethod method)
    {
        Check.NotNull(history, nameof(history));
        Check.NotNullOrWhiteSpace(transactionId, nameof(transactionId));

        if (paymentReceived < 0)
            throw new PaymentReceivedNegativeException(paymentReceived);

        var existing = await _paymentRepository.FindByTransactionIdAsync(transactionId);
        if (existing != null && existing.Id != history.Id)
            throw new MaintenancePaymentHistoryAlreadyExistsException(transactionId);

        var bill = await _billRepository.GetAsync(history.MaintenanceBillId);

        var arrears = bill.PaymentBeforeDueDate - paymentReceived;

        if (arrears == 0)
        {
            bill.Status = BillStatus.Paid;
            bill.Arrears = 0;
        }
        else
        {
            bill.Status = BillStatus.PartiallyPaid;
            bill.Arrears = arrears;
        }

        history.UpdateTransaction(transactionId);
        history.PaymentReceived = paymentReceived;
        history.PaymentDate = paymentDate;
        history.Method = method;

        await _paymentRepository.UpdateAsync(history);
        await _billRepository.UpdateAsync(bill);
    }
}



