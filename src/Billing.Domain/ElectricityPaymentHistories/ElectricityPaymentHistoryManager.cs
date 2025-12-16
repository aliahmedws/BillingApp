using Billing.ElectricityBills;
using Billing.MaintenanceBills;
using Billing.MaintenancePaymentHistories;
using System;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Domain.Services;

namespace Billing.ElectricityPaymentHistories;
public class ElectricityPaymentHistoryManager : DomainService
{
    private readonly IElectricityPaymentHistoryRepository _electricityPaymentHistoryRepository;
    private readonly IElectricityBillRepository _electricityBillRepository;

    public ElectricityPaymentHistoryManager(
        IElectricityPaymentHistoryRepository electricityPaymentHistoryRepository,
        IElectricityBillRepository electricityBillRepository)
    {
        _electricityPaymentHistoryRepository = electricityPaymentHistoryRepository;
        _electricityBillRepository = electricityBillRepository;
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

        var bill = await _electricityBillRepository.GetAsync(electricityBillId);

        var arrears = bill.PayableDueDateAmount - paymentReceived;

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
        Check.NotNullOrWhiteSpace(transactionId, nameof(transactionId));

        if (paymentReceived < 0)
        {
            throw new BillAmountRangeException(paymentReceived);
        }

        var existingPayment = await _electricityPaymentHistoryRepository.FindByTransactionIdAsync(transactionId);
        if (existingPayment != null && existingPayment.Id != paymentHistory.Id)
        {
            throw new ElectricityPaymentAlreadyExistsException(transactionId);
        }

        var bill = await _electricityBillRepository.GetAsync(paymentHistory.ElectricityBillId);

        var arrears = bill.PayableAfterDueDateAmount - paymentReceived;

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

        paymentHistory.ChangeTransactionId(transactionId); 
        paymentHistory.ChangePaymentReceived(paymentReceived);
        paymentHistory.PaymentDate = paymentDate;
        paymentHistory.Method = method;
    }

}

