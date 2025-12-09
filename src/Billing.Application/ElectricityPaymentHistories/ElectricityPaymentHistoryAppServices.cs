using Billing.Permissions;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Billing.ElectricityPaymentHistories;

[RemoteService(false)]
[Authorize(BillingPermissions.ElectricityPaymentHistories.Default)]
public class ElectricityPaymentHistoryAppService : ApplicationService, IElectricityPaymentHistoryAppService
{
    private readonly IElectricityPaymentHistoryRepository _electricityPaymentHistoryRepository;
    private readonly ElectricityPaymentHistoryManager _electricityPaymentHistoryManager;

    public ElectricityPaymentHistoryAppService(
        IElectricityPaymentHistoryRepository electricityPaymentHistoryRepository,
        ElectricityPaymentHistoryManager electricityPaymentHistoryManager)
    {
        _electricityPaymentHistoryRepository = electricityPaymentHistoryRepository;
        _electricityPaymentHistoryManager = electricityPaymentHistoryManager;
    }

    public async Task<ElectricityPaymentHistoryDto> GetAsync(Guid id)
    {
        var paymentHistory = await _electricityPaymentHistoryRepository.GetAsync(id);
        return ObjectMapper.Map<ElectricityPaymentHistory, ElectricityPaymentHistoryDto>(paymentHistory);
    }

    public async Task<PagedResultDto<ElectricityPaymentHistoryDto>> GetListAsync(GetElectricityPaymentHistoryListDto input)
    {
        if (input.Sorting.IsNullOrWhiteSpace())
        {
            input.Sorting = nameof(ElectricityPaymentHistory.CreationTime);
        }

        var totalCount = await _electricityPaymentHistoryRepository.GetCountAsync(
          input.Filter,
          input.TransactionId,
          input.Method,
          input.ElectricityBillId
          );

        var items = await _electricityPaymentHistoryRepository.GetListAsync(
           input.SkipCount,
           input.MaxResultCount,
           input.Sorting,
           input.Filter,
           input.TransactionId,
           input.Method,
           input.ElectricityBillId
           );

        var itemsDto = ObjectMapper.Map<List<ElectricityPaymentHistory>, List<ElectricityPaymentHistoryDto>>(items);

        return new PagedResultDto<ElectricityPaymentHistoryDto>(
            totalCount,
            itemsDto
        );
    }

    [Authorize(BillingPermissions.ElectricityPaymentHistories.Create)]
    public async Task<ElectricityPaymentHistoryDto> CreateAsync(CreateElectricityPaymentHistoryDto input)
    {
        var existingPayment = await _electricityPaymentHistoryRepository.FindByTransactionIdAsync(input.TransactionId);
        if (existingPayment != null)
        {
            throw new ElectricityPaymentAlreadyExistsException(input.TransactionId);
        }

        var paymentHistory = await _electricityPaymentHistoryManager.CreateAsync(
            input.ElectricityBillId,
            input.TransactionId,
            input.PaymentReceived,
            input.PaymentDate,
            input.Method
        );

        await _electricityPaymentHistoryRepository.InsertAsync(paymentHistory);
        return ObjectMapper.Map<ElectricityPaymentHistory, ElectricityPaymentHistoryDto>(paymentHistory);
    }

    [Authorize(BillingPermissions.ElectricityPaymentHistories.Edit)]
    public async Task UpdateAsync(Guid id, UpdateElectricityPaymentHistoryDto input)
    {
        var paymentHistory = await _electricityPaymentHistoryRepository.GetAsync(id);

        await _electricityPaymentHistoryManager.UpdateElectricityPaymentHistoryAsync(
            paymentHistory,
            input.TransactionId,
            input.PaymentReceived,
            input.PaymentDate,
            input.Method
        );

        await _electricityPaymentHistoryRepository.UpdateAsync(paymentHistory);
    }

    [Authorize(BillingPermissions.ElectricityPaymentHistories.Delete)]
    public async Task DeleteAsync(Guid id)
    {
        var paymentHistory = await _electricityPaymentHistoryRepository.GetAsync(id);
        await _electricityPaymentHistoryRepository.DeleteAsync(paymentHistory);
    }
}
